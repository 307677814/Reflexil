﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Daniel Grunwald" email="daniel@danielgrunwald.de"/>
//     <version>$Revision: 2819 $</version>
// </file>

using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;

namespace ICSharpCode.SharpDevelop.Dom.NRefactoryResolver
{
	/// <summary>
	/// This class converts C# constructs to their VB.NET equivalents.
	/// </summary>
	public class CSharpToVBNetConvertVisitor : CSharpConstructsConvertVisitor
	{
		NRefactoryResolver resolver;
		ParseInformation parseInformation;
		IProjectContent projectContent;
		public string RootNamespaceToRemove { get; set; }
		public string StartupObjectToMakePublic { get; set; }
		public IList<string> DefaultImportsToRemove { get; set; }
		
		public CSharpToVBNetConvertVisitor(IProjectContent pc, ParseInformation parseInfo)
		{
			this.resolver = new NRefactoryResolver(LanguageProperties.CSharp);
			this.projectContent = pc;
			this.parseInformation = parseInfo;
		}
		
		public override object VisitCompilationUnit(CompilationUnit compilationUnit, object data)
		{
			base.VisitCompilationUnit(compilationUnit, data);
			ToVBNetConvertVisitor v = new ToVBNetConvertVisitor();
			compilationUnit.AcceptVisitor(v, data);
			return null;
		}
		
		IReturnType ResolveType(TypeReference typeRef)
		{
			return TypeVisitor.CreateReturnType(typeRef, resolver);
		}
		
		public override object VisitNamespaceDeclaration(NamespaceDeclaration namespaceDeclaration, object data)
		{
			if (RootNamespaceToRemove != null) {
				if (namespaceDeclaration.Name == RootNamespaceToRemove) {
					// remove namespace declaration
					INode insertAfter = namespaceDeclaration;
					foreach (INode child in namespaceDeclaration.Children) {
						InsertAfterSibling(insertAfter, child);
						insertAfter = child;
					}
					namespaceDeclaration.Children.Clear();
					RemoveCurrentNode();
				} else if (namespaceDeclaration.Name.StartsWith(RootNamespaceToRemove + ".")) {
					namespaceDeclaration.Name = namespaceDeclaration.Name.Substring(RootNamespaceToRemove.Length + 1);
				}
			}
			base.VisitNamespaceDeclaration(namespaceDeclaration, data);
			return null;
		}
		
		public override object VisitUsing(Using @using, object data)
		{
			base.VisitUsing(@using, data);
			if (DefaultImportsToRemove != null && !@using.IsAlias) {
				if (DefaultImportsToRemove.Contains(@using.Name)) {
					RemoveCurrentNode();
				}
			}
			return null;
		}
		
		public override object VisitUsingDeclaration(UsingDeclaration usingDeclaration, object data)
		{
			base.VisitUsingDeclaration(usingDeclaration, data);
			if (usingDeclaration.Usings.Count == 0) {
				RemoveCurrentNode();
			}
			return null;
		}
		
		struct BaseType
		{
			internal readonly TypeReference TypeReference;
			internal readonly IReturnType ReturnType;
			internal readonly IClass UnderlyingClass;
			
			public BaseType(TypeReference typeReference, IReturnType returnType)
			{
				this.TypeReference = typeReference;
				this.ReturnType = returnType;
				this.UnderlyingClass = returnType.GetUnderlyingClass();
			}
		}
		
		public override object VisitMethodDeclaration(MethodDeclaration methodDeclaration, object data)
		{
			// Initialize resolver for method:
			if (!methodDeclaration.Body.IsNull) {
				if (resolver.Initialize(parseInformation, methodDeclaration.Body.StartLocation.Y, methodDeclaration.Body.StartLocation.X)) {
					resolver.RunLookupTableVisitor(methodDeclaration);
				}
			}
			IMethod currentMethod = resolver.CallingMember as IMethod;
			CreateInterfaceImplementations(currentMethod, methodDeclaration, methodDeclaration.InterfaceImplementations);
			if (currentMethod != null && currentMethod.Name == "Main") {
				if (currentMethod.DeclaringType.FullyQualifiedName == StartupObjectToMakePublic) {
					if (currentMethod.IsStatic && currentMethod.IsPrivate) {
						methodDeclaration.Modifier &= ~Modifiers.Private;
						methodDeclaration.Modifier |= Modifiers.Internal;
					}
				}
			}
			return base.VisitMethodDeclaration(methodDeclaration, data);
		}
		
		ClassFinder CreateContext()
		{
			return new ClassFinder(resolver.CallingClass, resolver.CallingMember, resolver.CaretLine, resolver.CaretColumn);
		}
		
		void CreateInterfaceImplementations(IMember currentMember, ParametrizedNode memberDecl, List<InterfaceImplementation> interfaceImplementations)
		{
			if (currentMember != null
			    && (memberDecl.Modifier & Modifiers.Visibility) == Modifiers.None
			    && interfaceImplementations.Count == 1)
			{
				// member is explicitly implementing an interface member
				// to convert explicit interface implementations to VB, make the member private
				// and ensure its name does not collide with another member
				memberDecl.Modifier |= Modifiers.Private;
				memberDecl.Name = interfaceImplementations[0].InterfaceType.Type.Replace('.', '_') + "_" + memberDecl.Name;
			}
			
			if (currentMember != null && currentMember.IsPublic
			    && currentMember.DeclaringType.ClassType != ClassType.Interface)
			{
				// member could be implicitly implementing an interface member,
				// search for interfaces containing the member
				foreach (IReturnType directBaseType in currentMember.DeclaringType.GetCompoundClass().BaseTypes) {
					IClass directBaseClass = directBaseType.GetUnderlyingClass();
					if (directBaseClass != null && directBaseClass.ClassType == ClassType.Interface) {
						// include members inherited from other interfaces in the search:
						foreach (IReturnType baseType in MemberLookupHelper.GetTypeInheritanceTree(directBaseType)) {
							IClass baseClass = baseType.GetUnderlyingClass();
							if (baseClass != null && baseClass.ClassType == ClassType.Interface) {
								IMember similarMember = MemberLookupHelper.FindSimilarMember(baseClass, currentMember);
								// add an interface implementation for similarMember
								// only when similarMember is not explicitly implemented by another member in this class
								if (similarMember != null && !HasExplicitImplementationFor(similarMember, baseType, memberDecl.Parent)) {
									interfaceImplementations.Add(new InterfaceImplementation(
										Refactoring.CodeGenerator.ConvertType(baseType, CreateContext()),
										currentMember.Name));
								}
							}
						}
					}
				}
			}
		}
		
		bool HasExplicitImplementationFor(IMember interfaceMember, IReturnType interfaceReference, INode typeDecl)
		{
			if (typeDecl == null)
				return false;
			foreach (INode node in typeDecl.Children) {
				MemberNode memberNode = node as MemberNode;
				if (memberNode != null && memberNode.InterfaceImplementations.Count > 0) {
					foreach (InterfaceImplementation impl in memberNode.InterfaceImplementations) {
						if (impl.MemberName == interfaceMember.Name
						    && object.Equals(ResolveType(impl.InterfaceType), interfaceReference)) {
							return true;
						}
					}
				}
			}
			return false;
		}
		
		public override object VisitConstructorDeclaration(ConstructorDeclaration constructorDeclaration, object data)
		{
			if (!constructorDeclaration.Body.IsNull) {
				if (resolver.Initialize(parseInformation, constructorDeclaration.Body.StartLocation.Y, constructorDeclaration.Body.StartLocation.X)) {
					resolver.RunLookupTableVisitor(constructorDeclaration);
				}
			}
			return base.VisitConstructorDeclaration(constructorDeclaration, data);
		}
		
		public override object VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration, object data)
		{
			if (resolver.Initialize(parseInformation, propertyDeclaration.BodyStart.Y, propertyDeclaration.BodyStart.X)) {
				resolver.RunLookupTableVisitor(propertyDeclaration);
			}
			IProperty currentProperty = resolver.CallingMember as IProperty;
			CreateInterfaceImplementations(currentProperty, propertyDeclaration, propertyDeclaration.InterfaceImplementations);
			return base.VisitPropertyDeclaration(propertyDeclaration, data);
		}
		
		public override object VisitExpressionStatement(ExpressionStatement expressionStatement, object data)
		{
			if (resolver.CompilationUnit == null)
				return base.VisitExpressionStatement(expressionStatement, data);
			
			// Transform event invocations that aren't already transformed by a parent IfStatement to RaiseEvent statement
			InvocationExpression eventInvocation = expressionStatement.Expression as InvocationExpression;
			if (eventInvocation != null && eventInvocation.TargetObject is IdentifierExpression) {
				MemberResolveResult mrr = resolver.ResolveInternal(eventInvocation.TargetObject, ExpressionContext.Default) as MemberResolveResult;
				if (mrr != null && mrr.ResolvedMember is IEvent) {
					ReplaceCurrentNode(new RaiseEventStatement(
						((IdentifierExpression)eventInvocation.TargetObject).Identifier,
						eventInvocation.Arguments));
				}
			}
			base.VisitExpressionStatement(expressionStatement, data);
			
			HandleAssignmentStatement(expressionStatement.Expression as AssignmentExpression);
			return null;
		}
		
		public override object VisitBinaryOperatorExpression(BinaryOperatorExpression binaryOperatorExpression, object data)
		{
			base.VisitBinaryOperatorExpression(binaryOperatorExpression, data);
			
			if (resolver.CompilationUnit == null)
				return null;
			
			switch (binaryOperatorExpression.Op) {
				case BinaryOperatorType.Equality:
				case BinaryOperatorType.InEquality:
					ConvertEqualityToReferenceEqualityIfRequired(binaryOperatorExpression);
					break;
				case BinaryOperatorType.Add:
					ConvertArgumentsForStringConcatenationIfRequired(binaryOperatorExpression);
					break;
				case BinaryOperatorType.Divide:
					ConvertDivisionToIntegerDivisionIfRequired(binaryOperatorExpression);
					break;
			}
			return null;
		}
		
		void ConvertEqualityToReferenceEqualityIfRequired(BinaryOperatorExpression binaryOperatorExpression)
		{
			// maybe we have to convert Equality operator to ReferenceEquality
			ResolveResult left = resolver.ResolveInternal(binaryOperatorExpression.Left, ExpressionContext.Default);
			ResolveResult right = resolver.ResolveInternal(binaryOperatorExpression.Right, ExpressionContext.Default);
			if (left != null && right != null && left.ResolvedType != null && right.ResolvedType != null) {
				IClass cLeft = left.ResolvedType.GetUnderlyingClass();
				IClass cRight = right.ResolvedType.GetUnderlyingClass();
				if (cLeft != null && cRight != null) {
					if ((cLeft.ClassType != ClassType.Struct && cLeft.ClassType != ClassType.Enum)
					    || (cRight.ClassType != ClassType.Struct && cRight.ClassType != ClassType.Enum))
					{
						// this is a reference comparison
						if (cLeft.FullyQualifiedName != "System.String") {
							// and it's not a string comparison, so we'll use reference equality
							if (binaryOperatorExpression.Op == BinaryOperatorType.Equality) {
								binaryOperatorExpression.Op = BinaryOperatorType.ReferenceEquality;
							} else {
								binaryOperatorExpression.Op = BinaryOperatorType.ReferenceInequality;
							}
						}
					}
				}
			}
		}
		
		void ConvertArgumentsForStringConcatenationIfRequired(BinaryOperatorExpression binaryOperatorExpression)
		{
			ResolveResult left = resolver.ResolveInternal(binaryOperatorExpression.Left, ExpressionContext.Default);
			ResolveResult right = resolver.ResolveInternal(binaryOperatorExpression.Right, ExpressionContext.Default);
			
			if (left != null && right != null) {
				if (IsString(left.ResolvedType)) {
					binaryOperatorExpression.Op = BinaryOperatorType.Concat;
					if (NeedsExplicitConversionToString(right.ResolvedType)) {
						binaryOperatorExpression.Right = CreateExplicitConversionToString(binaryOperatorExpression.Right);
					}
				} else if (IsString(right.ResolvedType)) {
					binaryOperatorExpression.Op = BinaryOperatorType.Concat;
					if (NeedsExplicitConversionToString(left.ResolvedType)) {
						binaryOperatorExpression.Left = CreateExplicitConversionToString(binaryOperatorExpression.Left);
					}
				}
			}
		}
		
		void ConvertDivisionToIntegerDivisionIfRequired(BinaryOperatorExpression binaryOperatorExpression)
		{
			ResolveResult left = resolver.ResolveInternal(binaryOperatorExpression.Left, ExpressionContext.Default);
			ResolveResult right = resolver.ResolveInternal(binaryOperatorExpression.Right, ExpressionContext.Default);
			
			if (left != null && right != null) {
				if (IsInteger(left.ResolvedType) && IsInteger(right.ResolvedType)) {
					binaryOperatorExpression.Op = BinaryOperatorType.DivideInteger;
				}
			}
		}
		
		bool IsString(IReturnType rt)
		{
			return rt != null && rt.IsDefaultReturnType && rt.FullyQualifiedName == "System.String";
		}
		
		bool IsInteger(IReturnType rt)
		{
			if (rt != null && rt.IsDefaultReturnType) {
				switch (rt.FullyQualifiedName) {
					case "System.Byte":
					case "System.SByte":
					case "System.Int16":
					case "System.UInt16":
					case "System.Int32":
					case "System.UInt32":
					case "System.Int64":
					case "System.UInt64":
						return true;
				}
			}
			return false;
		}
		
		bool NeedsExplicitConversionToString(IReturnType rt)
		{
			if (rt != null) {
				if (rt.IsDefaultReturnType) {
					if (rt.FullyQualifiedName == "System.Object"
					    || !TypeReference.PrimitiveTypesVBReverse.ContainsKey(rt.FullyQualifiedName))
					{
						// object and non-primitive types need explicit conversion
						return true;
					} else {
						// primitive types except object don't need explicit conversion
						return false;
					}
				} else {
					return true;
				}
			}
			return false;
		}
		
		Expression CreateExplicitConversionToString(Expression expr)
		{
			InvocationExpression ie = new InvocationExpression(
				new MemberReferenceExpression(new IdentifierExpression("Convert"), "ToString"));
			ie.Arguments.Add(expr);
			return ie;
		}
		
		public override object VisitIdentifierExpression(IdentifierExpression identifierExpression, object data)
		{
			base.VisitIdentifierExpression(identifierExpression, data);
			if (resolver.CompilationUnit == null)
				return null;
			
			InvocationExpression parentIE = identifierExpression.Parent as InvocationExpression;
			if (!(identifierExpression.Parent is AddressOfExpression)
			    && (parentIE == null || parentIE.TargetObject != identifierExpression))
			{
				ResolveResult rr = resolver.ResolveInternal(identifierExpression, ExpressionContext.Default);
				if (rr is MethodGroupResolveResult) {
					ReplaceCurrentNode(new AddressOfExpression(identifierExpression));
				}
			}
			return null;
		}
		
		public override object VisitMemberReferenceExpression(MemberReferenceExpression fieldReferenceExpression, object data)
		{
			base.VisitMemberReferenceExpression(fieldReferenceExpression, data);
			
			if (resolver.CompilationUnit == null)
				return null;
			
			InvocationExpression parentIE = fieldReferenceExpression.Parent as InvocationExpression;
			if (!(fieldReferenceExpression.Parent is AddressOfExpression)
			    && (parentIE == null || parentIE.TargetObject != fieldReferenceExpression))
			{
				ResolveResult rr = resolver.ResolveInternal(fieldReferenceExpression, ExpressionContext.Default);
				if (rr is MethodGroupResolveResult) {
					ReplaceCurrentNode(new AddressOfExpression(fieldReferenceExpression));
				}
			}
			
			return null;
		}
		
		void HandleAssignmentStatement(AssignmentExpression assignmentExpression)
		{
			if (resolver.CompilationUnit == null || assignmentExpression == null)
				return;
			
			if (assignmentExpression.Op == AssignmentOperatorType.Add || assignmentExpression.Op == AssignmentOperatorType.Subtract) {
				ResolveResult rr = resolver.ResolveInternal(assignmentExpression.Left, ExpressionContext.Default);
				if (rr is MemberResolveResult && (rr as MemberResolveResult).ResolvedMember is IEvent) {
					if (assignmentExpression.Op == AssignmentOperatorType.Add) {
						ReplaceCurrentNode(new AddHandlerStatement(assignmentExpression.Left, assignmentExpression.Right));
					} else {
						ReplaceCurrentNode(new RemoveHandlerStatement(assignmentExpression.Left, assignmentExpression.Right));
					}
				} else if (rr != null && rr.ResolvedType != null) {
					IClass c = rr.ResolvedType.GetUnderlyingClass();
					if (c.ClassType == ClassType.Delegate) {
						InvocationExpression invocation = new InvocationExpression(
							new MemberReferenceExpression(
								new IdentifierExpression("Delegate"),
								assignmentExpression.Op == AssignmentOperatorType.Add ? "Combine" : "Remove"));
						invocation.Arguments.Add(assignmentExpression.Left);
						invocation.Arguments.Add(assignmentExpression.Right);
						
						assignmentExpression.Op = AssignmentOperatorType.Assign;
						assignmentExpression.Right = new CastExpression(
							Refactoring.CodeGenerator.ConvertType(rr.ResolvedType, CreateContext()),
							invocation, CastType.Cast);
					}
				}
			}
		}
		
		public override object VisitCastExpression(CastExpression castExpression, object data)
		{
			base.VisitCastExpression(castExpression, data);
			
			if (resolver.CompilationUnit == null)
				return null;
			
			// cast to value type is a conversion
			if (castExpression.CastType == CastType.Cast) {
				IReturnType rt = ResolveType(castExpression.CastTo);
				if (rt != null) {
					IClass c = rt.GetUnderlyingClass();
					if (c != null && (c.ClassType == ClassType.Struct || c.ClassType == ClassType.Enum)) {
						castExpression.CastType = CastType.Conversion;
					}
				}
			}
			return null;
		}
	}
}
