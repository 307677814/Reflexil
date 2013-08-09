
#line  1 "cs.ATG" 
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using ICSharpCode.NRefactory.Parser;
using ICSharpCode.NRefactory.Ast;
using ASTAttribute = ICSharpCode.NRefactory.Ast.Attribute;
using Types = ICSharpCode.NRefactory.Ast.ClassType;
/*
  Parser.frame file for NRefactory.
 */
using System;
using System.Reflection;

namespace ICSharpCode.NRefactory.Parser.CSharp {



partial class Parser : AbstractParser
{
	const int maxT = 145;

	const  bool   T            = true;
	const  bool   x            = false;
	

#line  18 "cs.ATG" 


/*

*/

	void CS() {

#line  179 "cs.ATG" 
		lexer.NextToken(); /* get the first token */ 
		while (la.kind == 121) {
			UsingDirective();
		}
		while (
#line  182 "cs.ATG" 
IsGlobalAttrTarget()) {
			GlobalAttributeSection();
		}
		while (StartOf(1)) {
			NamespaceMemberDecl();
		}
		Expect(0);
	}

	void UsingDirective() {

#line  189 "cs.ATG" 
		string qualident = null; TypeReference aliasedType = null;
		
		Expect(121);

#line  192 "cs.ATG" 
		Location startPos = t.Location; 
		Qualident(
#line  193 "cs.ATG" 
out qualident);
		if (la.kind == 3) {
			lexer.NextToken();
			NonArrayType(
#line  194 "cs.ATG" 
out aliasedType);
		}
		Expect(11);

#line  196 "cs.ATG" 
		if (qualident != null && qualident.Length > 0) {
		 INode node;
		 if (aliasedType != null) {
		     node = new UsingDeclaration(qualident, aliasedType);
		 } else {
		     node = new UsingDeclaration(qualident);
		 }
		 node.StartLocation = startPos;
		 node.EndLocation   = t.EndLocation;
		 compilationUnit.AddChild(node);
		}
		
	}

	void GlobalAttributeSection() {
		Expect(18);

#line  212 "cs.ATG" 
		Location startPos = t.Location; 
		Identifier();

#line  213 "cs.ATG" 
		if (t.val != "assembly" && t.val != "module") Error("global attribute target specifier (assembly or module) expected");
		string attributeTarget = t.val;
		List<ASTAttribute> attributes = new List<ASTAttribute>();
		ASTAttribute attribute;
		
		Expect(9);
		Attribute(
#line  218 "cs.ATG" 
out attribute);

#line  218 "cs.ATG" 
		attributes.Add(attribute); 
		while (
#line  219 "cs.ATG" 
NotFinalComma()) {
			Expect(14);
			Attribute(
#line  219 "cs.ATG" 
out attribute);

#line  219 "cs.ATG" 
			attributes.Add(attribute); 
		}
		if (la.kind == 14) {
			lexer.NextToken();
		}
		Expect(19);

#line  221 "cs.ATG" 
		AttributeSection section = new AttributeSection {
		   AttributeTarget = attributeTarget,
		   Attributes = attributes,
		   StartLocation = startPos,
		   EndLocation = t.EndLocation
		};
		compilationUnit.AddChild(section);
		
	}

	void NamespaceMemberDecl() {

#line  322 "cs.ATG" 
		AttributeSection section;
		List<AttributeSection> attributes = new List<AttributeSection>();
		ModifierList m = new ModifierList();
		string qualident;
		
		if (la.kind == 88) {
			lexer.NextToken();

#line  328 "cs.ATG" 
			Location startPos = t.Location; 
			Qualident(
#line  329 "cs.ATG" 
out qualident);

#line  329 "cs.ATG" 
			INode node =  new NamespaceDeclaration(qualident);
			node.StartLocation = startPos;
			compilationUnit.AddChild(node);
			compilationUnit.BlockStart(node);
			
			Expect(16);
			while (la.kind == 121) {
				UsingDirective();
			}
			while (StartOf(1)) {
				NamespaceMemberDecl();
			}
			Expect(17);
			if (la.kind == 11) {
				lexer.NextToken();
			}

#line  338 "cs.ATG" 
			node.EndLocation   = t.EndLocation;
			compilationUnit.BlockEnd();
			
		} else if (StartOf(2)) {
			while (la.kind == 18) {
				AttributeSection(
#line  342 "cs.ATG" 
out section);

#line  342 "cs.ATG" 
				attributes.Add(section); 
			}
			while (StartOf(3)) {
				TypeModifier(
#line  343 "cs.ATG" 
m);
			}
			TypeDecl(
#line  344 "cs.ATG" 
m, attributes);
		} else SynErr(146);
	}

	void Qualident(
#line  468 "cs.ATG" 
out string qualident) {
		Identifier();

#line  470 "cs.ATG" 
		qualidentBuilder.Length = 0; qualidentBuilder.Append(t.val); 
		while (
#line  471 "cs.ATG" 
DotAndIdent()) {
			Expect(15);
			Identifier();

#line  471 "cs.ATG" 
			qualidentBuilder.Append('.');
			qualidentBuilder.Append(t.val); 
			
		}

#line  474 "cs.ATG" 
		qualident = qualidentBuilder.ToString(); 
	}

	void NonArrayType(
#line  583 "cs.ATG" 
out TypeReference type) {

#line  585 "cs.ATG" 
		string name;
		int pointer = 0;
		type = null;
		
		if (StartOf(4)) {
			ClassType(
#line  590 "cs.ATG" 
out type, false);
		} else if (StartOf(5)) {
			SimpleType(
#line  591 "cs.ATG" 
out name);

#line  591 "cs.ATG" 
			type = new TypeReference(name, true); 
		} else if (la.kind == 123) {
			lexer.NextToken();
			Expect(6);

#line  592 "cs.ATG" 
			pointer = 1; type = new TypeReference("System.Void", true); 
		} else SynErr(147);
		if (la.kind == 12) {
			NullableQuestionMark(
#line  595 "cs.ATG" 
ref type);
		}
		while (
#line  597 "cs.ATG" 
IsPointer()) {
			Expect(6);

#line  598 "cs.ATG" 
			++pointer; 
		}

#line  600 "cs.ATG" 
		if (type != null) { type.PointerNestingLevel = pointer; } 
	}

	void Identifier() {
		switch (la.kind) {
		case 1: {
			lexer.NextToken();
			break;
		}
		case 126: {
			lexer.NextToken();
			break;
		}
		case 127: {
			lexer.NextToken();
			break;
		}
		case 128: {
			lexer.NextToken();
			break;
		}
		case 129: {
			lexer.NextToken();
			break;
		}
		case 130: {
			lexer.NextToken();
			break;
		}
		case 131: {
			lexer.NextToken();
			break;
		}
		case 132: {
			lexer.NextToken();
			break;
		}
		case 133: {
			lexer.NextToken();
			break;
		}
		case 134: {
			lexer.NextToken();
			break;
		}
		case 135: {
			lexer.NextToken();
			break;
		}
		case 136: {
			lexer.NextToken();
			break;
		}
		case 137: {
			lexer.NextToken();
			break;
		}
		case 138: {
			lexer.NextToken();
			break;
		}
		case 139: {
			lexer.NextToken();
			break;
		}
		case 140: {
			lexer.NextToken();
			break;
		}
		case 141: {
			lexer.NextToken();
			break;
		}
		case 142: {
			lexer.NextToken();
			break;
		}
		case 143: {
			lexer.NextToken();
			break;
		}
		case 144: {
			lexer.NextToken();
			break;
		}
		default: SynErr(148); break;
		}
	}

	void Attribute(
#line  231 "cs.ATG" 
out ASTAttribute attribute) {

#line  232 "cs.ATG" 
		string qualident;
		string alias = null;
		

#line  236 "cs.ATG" 
		Location startPos = la.Location; 
		if (
#line  237 "cs.ATG" 
IdentAndDoubleColon()) {
			Identifier();

#line  238 "cs.ATG" 
			alias = t.val; 
			Expect(10);
		}
		Qualident(
#line  241 "cs.ATG" 
out qualident);

#line  242 "cs.ATG" 
		List<Expression> positional = new List<Expression>();
		List<NamedArgumentExpression> named = new List<NamedArgumentExpression>();
		string name = (alias != null && alias != "global") ? alias + "." + qualident : qualident;
		
		if (la.kind == 20) {
			AttributeArguments(
#line  246 "cs.ATG" 
positional, named);
		}

#line  247 "cs.ATG" 
		attribute = new ASTAttribute(name, positional, named); 
		attribute.StartLocation = startPos;
		attribute.EndLocation = t.EndLocation;
		
	}

	void AttributeArguments(
#line  253 "cs.ATG" 
List<Expression> positional, List<NamedArgumentExpression> named) {

#line  255 "cs.ATG" 
		bool nameFound = false;
		string name = "";
		Expression expr;
		
		Expect(20);
		if (StartOf(6)) {
			if (
#line  263 "cs.ATG" 
IsAssignment()) {

#line  263 "cs.ATG" 
				nameFound = true; 
				Identifier();

#line  264 "cs.ATG" 
				name = t.val; 
				Expect(3);
			}
			Expr(
#line  266 "cs.ATG" 
out expr);

#line  266 "cs.ATG" 
			if (expr != null) {if(name == "") positional.Add(expr);
			else { named.Add(new NamedArgumentExpression(name, expr)); name = ""; }
			}
			
			while (la.kind == 14) {
				lexer.NextToken();
				if (
#line  274 "cs.ATG" 
IsAssignment()) {

#line  274 "cs.ATG" 
					nameFound = true; 
					Identifier();

#line  275 "cs.ATG" 
					name = t.val; 
					Expect(3);
				} else if (StartOf(6)) {

#line  277 "cs.ATG" 
					if (nameFound) Error("no positional argument after named argument"); 
				} else SynErr(149);
				Expr(
#line  278 "cs.ATG" 
out expr);

#line  278 "cs.ATG" 
				if (expr != null) { if(name == "") positional.Add(expr);
				else { named.Add(new NamedArgumentExpression(name, expr)); name = ""; }
				}
				
			}
		}
		Expect(21);
	}

	void Expr(
#line  1734 "cs.ATG" 
out Expression expr) {

#line  1735 "cs.ATG" 
		expr = null; Expression expr1 = null, expr2 = null; AssignmentOperatorType op; 

#line  1737 "cs.ATG" 
		Location startLocation = la.Location; 
		UnaryExpr(
#line  1738 "cs.ATG" 
out expr);
		if (StartOf(7)) {
			AssignmentOperator(
#line  1741 "cs.ATG" 
out op);
			Expr(
#line  1741 "cs.ATG" 
out expr1);

#line  1741 "cs.ATG" 
			expr = new AssignmentExpression(expr, op, expr1); 
		} else if (
#line  1742 "cs.ATG" 
la.kind == Tokens.GreaterThan && Peek(1).kind == Tokens.GreaterEqual) {
			AssignmentOperator(
#line  1743 "cs.ATG" 
out op);
			Expr(
#line  1743 "cs.ATG" 
out expr1);

#line  1743 "cs.ATG" 
			expr = new AssignmentExpression(expr, op, expr1); 
		} else if (StartOf(8)) {
			ConditionalOrExpr(
#line  1745 "cs.ATG" 
ref expr);
			if (la.kind == 13) {
				lexer.NextToken();
				Expr(
#line  1746 "cs.ATG" 
out expr1);

#line  1746 "cs.ATG" 
				expr = new BinaryOperatorExpression(expr, BinaryOperatorType.NullCoalescing, expr1); 
			}
			if (la.kind == 12) {
				lexer.NextToken();
				Expr(
#line  1747 "cs.ATG" 
out expr1);
				Expect(9);
				Expr(
#line  1747 "cs.ATG" 
out expr2);

#line  1747 "cs.ATG" 
				expr = new ConditionalExpression(expr, expr1, expr2);  
			}
		} else SynErr(150);

#line  1750 "cs.ATG" 
		if (expr != null) {
		expr.StartLocation = startLocation;
		expr.EndLocation = t.EndLocation;
		}
		
	}

	void AttributeSection(
#line  287 "cs.ATG" 
out AttributeSection section) {

#line  289 "cs.ATG" 
		string attributeTarget = "";
		List<ASTAttribute> attributes = new List<ASTAttribute>();
		ASTAttribute attribute;
		
		
		Expect(18);

#line  295 "cs.ATG" 
		Location startPos = t.Location; 
		if (
#line  296 "cs.ATG" 
IsLocalAttrTarget()) {
			if (la.kind == 69) {
				lexer.NextToken();

#line  297 "cs.ATG" 
				attributeTarget = "event";
			} else if (la.kind == 101) {
				lexer.NextToken();

#line  298 "cs.ATG" 
				attributeTarget = "return";
			} else {
				Identifier();

#line  299 "cs.ATG" 
				if (t.val != "field"   && t.val != "method" &&
				  t.val != "param" &&
				  t.val != "property" && t.val != "type")
				Error("attribute target specifier (field, event, method, param, property, return or type) expected");
				attributeTarget = t.val;
				
			}
			Expect(9);
		}
		Attribute(
#line  308 "cs.ATG" 
out attribute);

#line  308 "cs.ATG" 
		attributes.Add(attribute); 
		while (
#line  309 "cs.ATG" 
NotFinalComma()) {
			Expect(14);
			Attribute(
#line  309 "cs.ATG" 
out attribute);

#line  309 "cs.ATG" 
			attributes.Add(attribute); 
		}
		if (la.kind == 14) {
			lexer.NextToken();
		}
		Expect(19);

#line  311 "cs.ATG" 
		section = new AttributeSection {
		   AttributeTarget = attributeTarget,
		   Attributes = attributes,
		   StartLocation = startPos,
		   EndLocation = t.EndLocation
		};
		
	}

	void TypeModifier(
#line  670 "cs.ATG" 
ModifierList m) {
		switch (la.kind) {
		case 89: {
			lexer.NextToken();

#line  672 "cs.ATG" 
			m.Add(Modifiers.New, t.Location); 
			break;
		}
		case 98: {
			lexer.NextToken();

#line  673 "cs.ATG" 
			m.Add(Modifiers.Public, t.Location); 
			break;
		}
		case 97: {
			lexer.NextToken();

#line  674 "cs.ATG" 
			m.Add(Modifiers.Protected, t.Location); 
			break;
		}
		case 84: {
			lexer.NextToken();

#line  675 "cs.ATG" 
			m.Add(Modifiers.Internal, t.Location); 
			break;
		}
		case 96: {
			lexer.NextToken();

#line  676 "cs.ATG" 
			m.Add(Modifiers.Private, t.Location); 
			break;
		}
		case 119: {
			lexer.NextToken();

#line  677 "cs.ATG" 
			m.Add(Modifiers.Unsafe, t.Location); 
			break;
		}
		case 49: {
			lexer.NextToken();

#line  678 "cs.ATG" 
			m.Add(Modifiers.Abstract, t.Location); 
			break;
		}
		case 103: {
			lexer.NextToken();

#line  679 "cs.ATG" 
			m.Add(Modifiers.Sealed, t.Location); 
			break;
		}
		case 107: {
			lexer.NextToken();

#line  680 "cs.ATG" 
			m.Add(Modifiers.Static, t.Location); 
			break;
		}
		case 126: {
			lexer.NextToken();

#line  681 "cs.ATG" 
			m.Add(Modifiers.Partial, t.Location); 
			break;
		}
		default: SynErr(151); break;
		}
	}

	void TypeDecl(
#line  347 "cs.ATG" 
ModifierList m, List<AttributeSection> attributes) {

#line  349 "cs.ATG" 
		TypeReference type;
		List<TypeReference> names;
		List<ParameterDeclarationExpression> p = new List<ParameterDeclarationExpression>();
		string name;
		List<TemplateDefinition> templates;
		
		if (la.kind == 59) {

#line  355 "cs.ATG" 
			m.Check(Modifiers.Classes); 
			lexer.NextToken();

#line  356 "cs.ATG" 
			TypeDeclaration newType = new TypeDeclaration(m.Modifier, attributes);
			templates = newType.Templates;
			compilationUnit.AddChild(newType);
			compilationUnit.BlockStart(newType);
			newType.StartLocation = m.GetDeclarationLocation(t.Location);
			
			newType.Type = Types.Class;
			
			Identifier();

#line  364 "cs.ATG" 
			newType.Name = t.val; 
			if (la.kind == 23) {
				TypeParameterList(
#line  367 "cs.ATG" 
templates);
			}
			if (la.kind == 9) {
				ClassBase(
#line  369 "cs.ATG" 
out names);

#line  369 "cs.ATG" 
				newType.BaseTypes = names; 
			}
			while (la.kind == 127) {
				TypeParameterConstraintsClause(
#line  372 "cs.ATG" 
templates);
			}

#line  374 "cs.ATG" 
			newType.BodyStartLocation = t.EndLocation; 
			Expect(16);
			ClassBody();
			Expect(17);
			if (la.kind == 11) {
				lexer.NextToken();
			}

#line  378 "cs.ATG" 
			newType.EndLocation = t.Location; 
			compilationUnit.BlockEnd();
			
		} else if (StartOf(9)) {

#line  381 "cs.ATG" 
			m.Check(Modifiers.StructsInterfacesEnumsDelegates); 
			if (la.kind == 109) {
				lexer.NextToken();

#line  382 "cs.ATG" 
				TypeDeclaration newType = new TypeDeclaration(m.Modifier, attributes);
				templates = newType.Templates;
				newType.StartLocation = m.GetDeclarationLocation(t.Location);
				compilationUnit.AddChild(newType);
				compilationUnit.BlockStart(newType);
				newType.Type = Types.Struct; 
				
				Identifier();

#line  389 "cs.ATG" 
				newType.Name = t.val; 
				if (la.kind == 23) {
					TypeParameterList(
#line  392 "cs.ATG" 
templates);
				}
				if (la.kind == 9) {
					StructInterfaces(
#line  394 "cs.ATG" 
out names);

#line  394 "cs.ATG" 
					newType.BaseTypes = names; 
				}
				while (la.kind == 127) {
					TypeParameterConstraintsClause(
#line  397 "cs.ATG" 
templates);
				}

#line  400 "cs.ATG" 
				newType.BodyStartLocation = t.EndLocation; 
				StructBody();
				if (la.kind == 11) {
					lexer.NextToken();
				}

#line  402 "cs.ATG" 
				newType.EndLocation = t.Location; 
				compilationUnit.BlockEnd();
				
			} else if (la.kind == 83) {
				lexer.NextToken();

#line  406 "cs.ATG" 
				TypeDeclaration newType = new TypeDeclaration(m.Modifier, attributes);
				templates = newType.Templates;
				compilationUnit.AddChild(newType);
				compilationUnit.BlockStart(newType);
				newType.StartLocation = m.GetDeclarationLocation(t.Location);
				newType.Type = Types.Interface;
				
				Identifier();

#line  413 "cs.ATG" 
				newType.Name = t.val; 
				if (la.kind == 23) {
					TypeParameterList(
#line  416 "cs.ATG" 
templates);
				}
				if (la.kind == 9) {
					InterfaceBase(
#line  418 "cs.ATG" 
out names);

#line  418 "cs.ATG" 
					newType.BaseTypes = names; 
				}
				while (la.kind == 127) {
					TypeParameterConstraintsClause(
#line  421 "cs.ATG" 
templates);
				}

#line  423 "cs.ATG" 
				newType.BodyStartLocation = t.EndLocation; 
				InterfaceBody();
				if (la.kind == 11) {
					lexer.NextToken();
				}

#line  425 "cs.ATG" 
				newType.EndLocation = t.Location; 
				compilationUnit.BlockEnd();
				
			} else if (la.kind == 68) {
				lexer.NextToken();

#line  429 "cs.ATG" 
				TypeDeclaration newType = new TypeDeclaration(m.Modifier, attributes);
				compilationUnit.AddChild(newType);
				compilationUnit.BlockStart(newType);
				newType.StartLocation = m.GetDeclarationLocation(t.Location);
				newType.Type = Types.Enum;
				
				Identifier();

#line  435 "cs.ATG" 
				newType.Name = t.val; 
				if (la.kind == 9) {
					lexer.NextToken();
					IntegralType(
#line  436 "cs.ATG" 
out name);

#line  436 "cs.ATG" 
					newType.BaseTypes.Add(new TypeReference(name, true)); 
				}

#line  438 "cs.ATG" 
				newType.BodyStartLocation = t.EndLocation; 
				EnumBody();
				if (la.kind == 11) {
					lexer.NextToken();
				}

#line  440 "cs.ATG" 
				newType.EndLocation = t.Location; 
				compilationUnit.BlockEnd();
				
			} else {
				lexer.NextToken();

#line  444 "cs.ATG" 
				DelegateDeclaration delegateDeclr = new DelegateDeclaration(m.Modifier, attributes);
				templates = delegateDeclr.Templates;
				delegateDeclr.StartLocation = m.GetDeclarationLocation(t.Location);
				
				if (
#line  448 "cs.ATG" 
NotVoidPointer()) {
					Expect(123);

#line  448 "cs.ATG" 
					delegateDeclr.ReturnType = new TypeReference("System.Void", true); 
				} else if (StartOf(10)) {
					Type(
#line  449 "cs.ATG" 
out type);

#line  449 "cs.ATG" 
					delegateDeclr.ReturnType = type; 
				} else SynErr(152);
				Identifier();

#line  451 "cs.ATG" 
				delegateDeclr.Name = t.val; 
				if (la.kind == 23) {
					TypeParameterList(
#line  454 "cs.ATG" 
templates);
				}
				Expect(20);
				if (StartOf(11)) {
					FormalParameterList(
#line  456 "cs.ATG" 
p);

#line  456 "cs.ATG" 
					delegateDeclr.Parameters = p; 
				}
				Expect(21);
				while (la.kind == 127) {
					TypeParameterConstraintsClause(
#line  460 "cs.ATG" 
templates);
				}
				Expect(11);

#line  462 "cs.ATG" 
				delegateDeclr.EndLocation = t.Location;
				compilationUnit.AddChild(delegateDeclr);
				
			}
		} else SynErr(153);
	}

	void TypeParameterList(
#line  2304 "cs.ATG" 
List<TemplateDefinition> templates) {

#line  2306 "cs.ATG" 
		AttributeSection section;
		List<AttributeSection> attributes = new List<AttributeSection>();
		
		Expect(23);
		while (la.kind == 18) {
			AttributeSection(
#line  2310 "cs.ATG" 
out section);

#line  2310 "cs.ATG" 
			attributes.Add(section); 
		}
		Identifier();

#line  2311 "cs.ATG" 
		templates.Add(new TemplateDefinition(t.val, attributes)); 
		while (la.kind == 14) {
			lexer.NextToken();
			while (la.kind == 18) {
				AttributeSection(
#line  2312 "cs.ATG" 
out section);

#line  2312 "cs.ATG" 
				attributes.Add(section); 
			}
			Identifier();

#line  2313 "cs.ATG" 
			templates.Add(new TemplateDefinition(t.val, attributes)); 
		}
		Expect(22);
	}

	void ClassBase(
#line  477 "cs.ATG" 
out List<TypeReference> names) {

#line  479 "cs.ATG" 
		TypeReference typeRef;
		names = new List<TypeReference>();
		
		Expect(9);
		ClassType(
#line  483 "cs.ATG" 
out typeRef, false);

#line  483 "cs.ATG" 
		if (typeRef != null) { names.Add(typeRef); } 
		while (la.kind == 14) {
			lexer.NextToken();
			TypeName(
#line  484 "cs.ATG" 
out typeRef, false);

#line  484 "cs.ATG" 
			if (typeRef != null) { names.Add(typeRef); } 
		}
	}

	void TypeParameterConstraintsClause(
#line  2317 "cs.ATG" 
List<TemplateDefinition> templates) {

#line  2318 "cs.ATG" 
		string name = ""; TypeReference type; 
		Expect(127);
		Identifier();

#line  2321 "cs.ATG" 
		name = t.val; 
		Expect(9);
		TypeParameterConstraintsClauseBase(
#line  2323 "cs.ATG" 
out type);

#line  2324 "cs.ATG" 
		TemplateDefinition td = null;
		foreach (TemplateDefinition d in templates) {
			if (d.Name == name) {
				td = d;
				break;
			}
		}
		if ( td != null && type != null) { td.Bases.Add(type); }
		
		while (la.kind == 14) {
			lexer.NextToken();
			TypeParameterConstraintsClauseBase(
#line  2333 "cs.ATG" 
out type);

#line  2334 "cs.ATG" 
			td = null;
			foreach (TemplateDefinition d in templates) {
				if (d.Name == name) {
					td = d;
					break;
				}
			}
			if ( td != null && type != null) { td.Bases.Add(type); }
			
		}
	}

	void ClassBody() {

#line  488 "cs.ATG" 
		AttributeSection section; 
		while (StartOf(12)) {

#line  490 "cs.ATG" 
			List<AttributeSection> attributes = new List<AttributeSection>();
			ModifierList m = new ModifierList();
			
			while (!(StartOf(13))) {SynErr(154); lexer.NextToken(); }
			while (la.kind == 18) {
				AttributeSection(
#line  494 "cs.ATG" 
out section);

#line  494 "cs.ATG" 
				attributes.Add(section); 
			}
			MemberModifiers(
#line  495 "cs.ATG" 
m);
			ClassMemberDecl(
#line  496 "cs.ATG" 
m, attributes);
		}
	}

	void StructInterfaces(
#line  500 "cs.ATG" 
out List<TypeReference> names) {

#line  502 "cs.ATG" 
		TypeReference typeRef;
		names = new List<TypeReference>();
		
		Expect(9);
		TypeName(
#line  506 "cs.ATG" 
out typeRef, false);

#line  506 "cs.ATG" 
		if (typeRef != null) { names.Add(typeRef); } 
		while (la.kind == 14) {
			lexer.NextToken();
			TypeName(
#line  507 "cs.ATG" 
out typeRef, false);

#line  507 "cs.ATG" 
			if (typeRef != null) { names.Add(typeRef); } 
		}
	}

	void StructBody() {

#line  511 "cs.ATG" 
		AttributeSection section; 
		Expect(16);
		while (StartOf(14)) {

#line  514 "cs.ATG" 
			List<AttributeSection> attributes = new List<AttributeSection>();
			ModifierList m = new ModifierList();
			
			while (la.kind == 18) {
				AttributeSection(
#line  517 "cs.ATG" 
out section);

#line  517 "cs.ATG" 
				attributes.Add(section); 
			}
			MemberModifiers(
#line  518 "cs.ATG" 
m);
			StructMemberDecl(
#line  519 "cs.ATG" 
m, attributes);
		}
		Expect(17);
	}

	void InterfaceBase(
#line  524 "cs.ATG" 
out List<TypeReference> names) {

#line  526 "cs.ATG" 
		TypeReference typeRef;
		names = new List<TypeReference>();
		
		Expect(9);
		TypeName(
#line  530 "cs.ATG" 
out typeRef, false);

#line  530 "cs.ATG" 
		if (typeRef != null) { names.Add(typeRef); } 
		while (la.kind == 14) {
			lexer.NextToken();
			TypeName(
#line  531 "cs.ATG" 
out typeRef, false);

#line  531 "cs.ATG" 
			if (typeRef != null) { names.Add(typeRef); } 
		}
	}

	void InterfaceBody() {
		Expect(16);
		while (StartOf(15)) {
			while (!(StartOf(16))) {SynErr(155); lexer.NextToken(); }
			InterfaceMemberDecl();
		}
		Expect(17);
	}

	void IntegralType(
#line  692 "cs.ATG" 
out string name) {

#line  692 "cs.ATG" 
		name = ""; 
		switch (la.kind) {
		case 102: {
			lexer.NextToken();

#line  694 "cs.ATG" 
			name = "System.SByte"; 
			break;
		}
		case 54: {
			lexer.NextToken();

#line  695 "cs.ATG" 
			name = "System.Byte"; 
			break;
		}
		case 104: {
			lexer.NextToken();

#line  696 "cs.ATG" 
			name = "System.Int16"; 
			break;
		}
		case 120: {
			lexer.NextToken();

#line  697 "cs.ATG" 
			name = "System.UInt16"; 
			break;
		}
		case 82: {
			lexer.NextToken();

#line  698 "cs.ATG" 
			name = "System.Int32"; 
			break;
		}
		case 116: {
			lexer.NextToken();

#line  699 "cs.ATG" 
			name = "System.UInt32"; 
			break;
		}
		case 87: {
			lexer.NextToken();

#line  700 "cs.ATG" 
			name = "System.Int64"; 
			break;
		}
		case 117: {
			lexer.NextToken();

#line  701 "cs.ATG" 
			name = "System.UInt64"; 
			break;
		}
		case 57: {
			lexer.NextToken();

#line  702 "cs.ATG" 
			name = "System.Char"; 
			break;
		}
		default: SynErr(156); break;
		}
	}

	void EnumBody() {

#line  540 "cs.ATG" 
		FieldDeclaration f; 
		Expect(16);
		if (StartOf(17)) {
			EnumMemberDecl(
#line  543 "cs.ATG" 
out f);

#line  543 "cs.ATG" 
			compilationUnit.AddChild(f); 
			while (
#line  544 "cs.ATG" 
NotFinalComma()) {
				Expect(14);
				EnumMemberDecl(
#line  545 "cs.ATG" 
out f);

#line  545 "cs.ATG" 
				compilationUnit.AddChild(f); 
			}
			if (la.kind == 14) {
				lexer.NextToken();
			}
		}
		Expect(17);
	}

	void Type(
#line  551 "cs.ATG" 
out TypeReference type) {
		TypeWithRestriction(
#line  553 "cs.ATG" 
out type, true, false);
	}

	void FormalParameterList(
#line  614 "cs.ATG" 
List<ParameterDeclarationExpression> parameter) {

#line  617 "cs.ATG" 
		ParameterDeclarationExpression p;
		AttributeSection section;
		List<AttributeSection> attributes = new List<AttributeSection>();
		
		while (la.kind == 18) {
			AttributeSection(
#line  622 "cs.ATG" 
out section);

#line  622 "cs.ATG" 
			attributes.Add(section); 
		}
		if (StartOf(18)) {
			FixedParameter(
#line  624 "cs.ATG" 
out p);

#line  624 "cs.ATG" 
			bool paramsFound = false;
			p.Attributes = attributes;
			parameter.Add(p);
			
			while (la.kind == 14) {
				lexer.NextToken();

#line  629 "cs.ATG" 
				attributes = new List<AttributeSection>(); if (paramsFound) Error("params array must be at end of parameter list"); 
				while (la.kind == 18) {
					AttributeSection(
#line  630 "cs.ATG" 
out section);

#line  630 "cs.ATG" 
					attributes.Add(section); 
				}
				if (StartOf(18)) {
					FixedParameter(
#line  632 "cs.ATG" 
out p);

#line  632 "cs.ATG" 
					p.Attributes = attributes; parameter.Add(p); 
				} else if (la.kind == 95) {
					ParameterArray(
#line  633 "cs.ATG" 
out p);

#line  633 "cs.ATG" 
					paramsFound = true; p.Attributes = attributes; parameter.Add(p); 
				} else SynErr(157);
			}
		} else if (la.kind == 95) {
			ParameterArray(
#line  636 "cs.ATG" 
out p);

#line  636 "cs.ATG" 
			p.Attributes = attributes; parameter.Add(p); 
		} else SynErr(158);
	}

	void ClassType(
#line  684 "cs.ATG" 
out TypeReference typeRef, bool canBeUnbound) {

#line  685 "cs.ATG" 
		TypeReference r; typeRef = null; 
		if (StartOf(19)) {
			TypeName(
#line  687 "cs.ATG" 
out r, canBeUnbound);

#line  687 "cs.ATG" 
			typeRef = r; 
		} else if (la.kind == 91) {
			lexer.NextToken();

#line  688 "cs.ATG" 
			typeRef = new TypeReference("System.Object", true); 
		} else if (la.kind == 108) {
			lexer.NextToken();

#line  689 "cs.ATG" 
			typeRef = new TypeReference("System.String", true); 
		} else SynErr(159);
	}

	void TypeName(
#line  2247 "cs.ATG" 
out TypeReference typeRef, bool canBeUnbound) {

#line  2248 "cs.ATG" 
		List<TypeReference> typeArguments = null;
		string alias = null;
		string qualident;
		
		if (
#line  2253 "cs.ATG" 
IdentAndDoubleColon()) {
			Identifier();

#line  2254 "cs.ATG" 
			alias = t.val; 
			Expect(10);
		}
		Qualident(
#line  2257 "cs.ATG" 
out qualident);
		if (la.kind == 23) {
			TypeArgumentList(
#line  2258 "cs.ATG" 
out typeArguments, canBeUnbound);
		}

#line  2260 "cs.ATG" 
		if (alias == null) {
		typeRef = new TypeReference(qualident, typeArguments);
		} else if (alias == "global") {
			typeRef = new TypeReference(qualident, typeArguments);
			typeRef.IsGlobal = true;
		} else {
			typeRef = new TypeReference(alias + "." + qualident, typeArguments);
		}
		
		while (
#line  2269 "cs.ATG" 
DotAndIdent()) {
			Expect(15);

#line  2270 "cs.ATG" 
			typeArguments = null; 
			Qualident(
#line  2271 "cs.ATG" 
out qualident);
			if (la.kind == 23) {
				TypeArgumentList(
#line  2272 "cs.ATG" 
out typeArguments, canBeUnbound);
			}

#line  2273 "cs.ATG" 
			typeRef = new InnerClassTypeReference(typeRef, qualident, typeArguments); 
		}
	}

	void MemberModifiers(
#line  705 "cs.ATG" 
ModifierList m) {
		while (StartOf(20)) {
			switch (la.kind) {
			case 49: {
				lexer.NextToken();

#line  708 "cs.ATG" 
				m.Add(Modifiers.Abstract, t.Location); 
				break;
			}
			case 71: {
				lexer.NextToken();

#line  709 "cs.ATG" 
				m.Add(Modifiers.Extern, t.Location); 
				break;
			}
			case 84: {
				lexer.NextToken();

#line  710 "cs.ATG" 
				m.Add(Modifiers.Internal, t.Location); 
				break;
			}
			case 89: {
				lexer.NextToken();

#line  711 "cs.ATG" 
				m.Add(Modifiers.New, t.Location); 
				break;
			}
			case 94: {
				lexer.NextToken();

#line  712 "cs.ATG" 
				m.Add(Modifiers.Override, t.Location); 
				break;
			}
			case 96: {
				lexer.NextToken();

#line  713 "cs.ATG" 
				m.Add(Modifiers.Private, t.Location); 
				break;
			}
			case 97: {
				lexer.NextToken();

#line  714 "cs.ATG" 
				m.Add(Modifiers.Protected, t.Location); 
				break;
			}
			case 98: {
				lexer.NextToken();

#line  715 "cs.ATG" 
				m.Add(Modifiers.Public, t.Location); 
				break;
			}
			case 99: {
				lexer.NextToken();

#line  716 "cs.ATG" 
				m.Add(Modifiers.ReadOnly, t.Location); 
				break;
			}
			case 103: {
				lexer.NextToken();

#line  717 "cs.ATG" 
				m.Add(Modifiers.Sealed, t.Location); 
				break;
			}
			case 107: {
				lexer.NextToken();

#line  718 "cs.ATG" 
				m.Add(Modifiers.Static, t.Location); 
				break;
			}
			case 74: {
				lexer.NextToken();

#line  719 "cs.ATG" 
				m.Add(Modifiers.Fixed, t.Location); 
				break;
			}
			case 119: {
				lexer.NextToken();

#line  720 "cs.ATG" 
				m.Add(Modifiers.Unsafe, t.Location); 
				break;
			}
			case 122: {
				lexer.NextToken();

#line  721 "cs.ATG" 
				m.Add(Modifiers.Virtual, t.Location); 
				break;
			}
			case 124: {
				lexer.NextToken();

#line  722 "cs.ATG" 
				m.Add(Modifiers.Volatile, t.Location); 
				break;
			}
			case 126: {
				lexer.NextToken();

#line  723 "cs.ATG" 
				m.Add(Modifiers.Partial, t.Location); 
				break;
			}
			}
		}
	}

	void ClassMemberDecl(
#line  1033 "cs.ATG" 
ModifierList m, List<AttributeSection> attributes) {

#line  1034 "cs.ATG" 
		Statement stmt = null; 
		if (StartOf(21)) {
			StructMemberDecl(
#line  1036 "cs.ATG" 
m, attributes);
		} else if (la.kind == 27) {

#line  1037 "cs.ATG" 
			m.Check(Modifiers.Destructors); Location startPos = t.Location; 
			lexer.NextToken();
			Identifier();

#line  1038 "cs.ATG" 
			DestructorDeclaration d = new DestructorDeclaration(t.val, m.Modifier, attributes); 
			d.Modifier = m.Modifier;
			d.StartLocation = m.GetDeclarationLocation(startPos);
			
			Expect(20);
			Expect(21);

#line  1042 "cs.ATG" 
			d.EndLocation = t.EndLocation; 
			if (la.kind == 16) {
				Block(
#line  1042 "cs.ATG" 
out stmt);
			} else if (la.kind == 11) {
				lexer.NextToken();
			} else SynErr(160);

#line  1043 "cs.ATG" 
			d.Body = (BlockStatement)stmt;
			compilationUnit.AddChild(d);
			
		} else SynErr(161);
	}

	void StructMemberDecl(
#line  727 "cs.ATG" 
ModifierList m, List<AttributeSection> attributes) {

#line  729 "cs.ATG" 
		string qualident = null;
		TypeReference type;
		Expression expr;
		List<ParameterDeclarationExpression> p = new List<ParameterDeclarationExpression>();
		Statement stmt = null;
		List<TemplateDefinition> templates = new List<TemplateDefinition>();
		TypeReference explicitInterface = null;
		bool isExtensionMethod = false;
		
		if (la.kind == 60) {

#line  739 "cs.ATG" 
			m.Check(Modifiers.Constants); 
			lexer.NextToken();

#line  740 "cs.ATG" 
			Location startPos = t.Location; 
			Type(
#line  741 "cs.ATG" 
out type);
			Identifier();

#line  741 "cs.ATG" 
			FieldDeclaration fd = new FieldDeclaration(attributes, type, m.Modifier | Modifiers.Const);
			fd.StartLocation = m.GetDeclarationLocation(startPos);
			VariableDeclaration f = new VariableDeclaration(t.val);
			SafeAdd(fd, fd.Fields, f);
			
			Expect(3);
			Expr(
#line  746 "cs.ATG" 
out expr);

#line  746 "cs.ATG" 
			f.Initializer = expr; 
			while (la.kind == 14) {
				lexer.NextToken();
				Identifier();

#line  747 "cs.ATG" 
				f = new VariableDeclaration(t.val);
				SafeAdd(fd, fd.Fields, f);
				
				Expect(3);
				Expr(
#line  750 "cs.ATG" 
out expr);

#line  750 "cs.ATG" 
				f.Initializer = expr; 
			}
			Expect(11);

#line  751 "cs.ATG" 
			fd.EndLocation = t.EndLocation; compilationUnit.AddChild(fd); 
		} else if (
#line  755 "cs.ATG" 
NotVoidPointer()) {

#line  755 "cs.ATG" 
			m.Check(Modifiers.PropertysEventsMethods); 
			Expect(123);

#line  756 "cs.ATG" 
			Location startPos = t.Location; 
			if (
#line  757 "cs.ATG" 
IsExplicitInterfaceImplementation()) {
				TypeName(
#line  758 "cs.ATG" 
out explicitInterface, false);

#line  759 "cs.ATG" 
				if (la.kind != Tokens.Dot || Peek(1).kind != Tokens.This) {
				qualident = TypeReference.StripLastIdentifierFromType(ref explicitInterface);
				 } 
			} else if (StartOf(19)) {
				Identifier();

#line  762 "cs.ATG" 
				qualident = t.val; 
			} else SynErr(162);
			if (la.kind == 23) {
				TypeParameterList(
#line  765 "cs.ATG" 
templates);
			}
			Expect(20);
			if (la.kind == 111) {
				lexer.NextToken();

#line  768 "cs.ATG" 
				isExtensionMethod = true; /* C# 3.0 */ 
			}
			if (StartOf(11)) {
				FormalParameterList(
#line  769 "cs.ATG" 
p);
			}
			Expect(21);

#line  770 "cs.ATG" 
			MethodDeclaration methodDeclaration = new MethodDeclaration {
			Name = qualident,
			Modifier = m.Modifier,
			TypeReference = new TypeReference("System.Void", true),
			Parameters = p,
			Attributes = attributes,
			StartLocation = m.GetDeclarationLocation(startPos),
			EndLocation = t.EndLocation,
			Templates = templates,
			IsExtensionMethod = isExtensionMethod
			};
			if (explicitInterface != null)
				methodDeclaration.InterfaceImplementations.Add(new InterfaceImplementation(explicitInterface, qualident));
			compilationUnit.AddChild(methodDeclaration);
			compilationUnit.BlockStart(methodDeclaration);
			
			while (la.kind == 127) {
				TypeParameterConstraintsClause(
#line  788 "cs.ATG" 
templates);
			}
			if (la.kind == 16) {
				Block(
#line  790 "cs.ATG" 
out stmt);
			} else if (la.kind == 11) {
				lexer.NextToken();
			} else SynErr(163);

#line  790 "cs.ATG" 
			compilationUnit.BlockEnd();
			methodDeclaration.Body  = (BlockStatement)stmt;
			
		} else if (la.kind == 69) {

#line  794 "cs.ATG" 
			m.Check(Modifiers.PropertysEventsMethods); 
			lexer.NextToken();

#line  796 "cs.ATG" 
			EventDeclaration eventDecl = new EventDeclaration {
			Modifier = m.Modifier, 
			Attributes = attributes,
			StartLocation = t.Location
			};
			compilationUnit.AddChild(eventDecl);
			compilationUnit.BlockStart(eventDecl);
			EventAddRegion addBlock = null;
			EventRemoveRegion removeBlock = null;
			
			Type(
#line  806 "cs.ATG" 
out type);

#line  806 "cs.ATG" 
			eventDecl.TypeReference = type; 
			if (
#line  807 "cs.ATG" 
IsExplicitInterfaceImplementation()) {
				TypeName(
#line  808 "cs.ATG" 
out explicitInterface, false);

#line  809 "cs.ATG" 
				qualident = TypeReference.StripLastIdentifierFromType(ref explicitInterface); 

#line  810 "cs.ATG" 
				eventDecl.InterfaceImplementations.Add(new InterfaceImplementation(explicitInterface, qualident)); 
			} else if (StartOf(19)) {
				Identifier();

#line  812 "cs.ATG" 
				qualident = t.val; 
			} else SynErr(164);

#line  814 "cs.ATG" 
			eventDecl.Name = qualident; eventDecl.EndLocation = t.EndLocation; 
			if (la.kind == 3) {
				lexer.NextToken();
				Expr(
#line  815 "cs.ATG" 
out expr);

#line  815 "cs.ATG" 
				eventDecl.Initializer = expr; 
			}
			if (la.kind == 16) {
				lexer.NextToken();

#line  816 "cs.ATG" 
				eventDecl.BodyStart = t.Location; 
				EventAccessorDecls(
#line  817 "cs.ATG" 
out addBlock, out removeBlock);
				Expect(17);

#line  818 "cs.ATG" 
				eventDecl.BodyEnd   = t.EndLocation; 
			}
			if (la.kind == 11) {
				lexer.NextToken();
			}

#line  821 "cs.ATG" 
			compilationUnit.BlockEnd();
			eventDecl.AddRegion = addBlock;
			eventDecl.RemoveRegion = removeBlock;
			
		} else if (
#line  827 "cs.ATG" 
IdentAndLPar()) {

#line  827 "cs.ATG" 
			m.Check(Modifiers.Constructors | Modifiers.StaticConstructors); 
			Identifier();

#line  828 "cs.ATG" 
			string name = t.val; Location startPos = t.Location; 
			Expect(20);
			if (StartOf(11)) {

#line  828 "cs.ATG" 
				m.Check(Modifiers.Constructors); 
				FormalParameterList(
#line  829 "cs.ATG" 
p);
			}
			Expect(21);

#line  831 "cs.ATG" 
			ConstructorInitializer init = null;  
			if (la.kind == 9) {

#line  832 "cs.ATG" 
				m.Check(Modifiers.Constructors); 
				ConstructorInitializer(
#line  833 "cs.ATG" 
out init);
			}

#line  835 "cs.ATG" 
			ConstructorDeclaration cd = new ConstructorDeclaration(name, m.Modifier, p, init, attributes); 
			cd.StartLocation = startPos;
			cd.EndLocation   = t.EndLocation;
			
			if (la.kind == 16) {
				Block(
#line  840 "cs.ATG" 
out stmt);
			} else if (la.kind == 11) {
				lexer.NextToken();
			} else SynErr(165);

#line  840 "cs.ATG" 
			cd.Body = (BlockStatement)stmt; compilationUnit.AddChild(cd); 
		} else if (la.kind == 70 || la.kind == 80) {

#line  843 "cs.ATG" 
			m.Check(Modifiers.Operators);
			if (m.isNone) Error("at least one modifier must be set"); 
			bool isImplicit = true;
			Location startPos = Location.Empty;
			
			if (la.kind == 80) {
				lexer.NextToken();

#line  848 "cs.ATG" 
				startPos = t.Location; 
			} else {
				lexer.NextToken();

#line  848 "cs.ATG" 
				isImplicit = false; startPos = t.Location; 
			}
			Expect(92);
			Type(
#line  849 "cs.ATG" 
out type);

#line  849 "cs.ATG" 
			TypeReference operatorType = type; 
			Expect(20);
			Type(
#line  850 "cs.ATG" 
out type);
			Identifier();

#line  850 "cs.ATG" 
			string varName = t.val; 
			Expect(21);

#line  851 "cs.ATG" 
			Location endPos = t.Location; 
			if (la.kind == 16) {
				Block(
#line  852 "cs.ATG" 
out stmt);
			} else if (la.kind == 11) {
				lexer.NextToken();

#line  852 "cs.ATG" 
				stmt = null; 
			} else SynErr(166);

#line  855 "cs.ATG" 
			List<ParameterDeclarationExpression> parameters = new List<ParameterDeclarationExpression>();
			parameters.Add(new ParameterDeclarationExpression(type, varName));
			OperatorDeclaration operatorDeclaration = new OperatorDeclaration {
				Modifier = m.Modifier,
				Attributes = attributes, 
				Parameters = parameters, 
				TypeReference = operatorType,
				ConversionType = isImplicit ? ConversionType.Implicit : ConversionType.Explicit,
				Body = (BlockStatement)stmt,
				StartLocation = m.GetDeclarationLocation(startPos),
				EndLocation = endPos
			};
			SetParent(parameters, operatorDeclaration);
			compilationUnit.AddChild(operatorDeclaration);
			
		} else if (StartOf(22)) {
			TypeDecl(
#line  873 "cs.ATG" 
m, attributes);
		} else if (StartOf(10)) {
			Type(
#line  875 "cs.ATG" 
out type);

#line  875 "cs.ATG" 
			Location startPos = t.Location;  
			if (la.kind == 92) {

#line  877 "cs.ATG" 
				OverloadableOperatorType op;
				m.Check(Modifiers.Operators);
				if (m.isNone) Error("at least one modifier must be set");
				
				lexer.NextToken();
				OverloadableOperator(
#line  881 "cs.ATG" 
out op);

#line  881 "cs.ATG" 
				TypeReference firstType, secondType = null; string secondName = null; 
				Expect(20);
				Type(
#line  882 "cs.ATG" 
out firstType);
				Identifier();

#line  882 "cs.ATG" 
				string firstName = t.val; 
				if (la.kind == 14) {
					lexer.NextToken();
					Type(
#line  883 "cs.ATG" 
out secondType);
					Identifier();

#line  883 "cs.ATG" 
					secondName = t.val; 
				} else if (la.kind == 21) {
				} else SynErr(167);

#line  891 "cs.ATG" 
				Location endPos = t.Location; 
				Expect(21);
				if (la.kind == 16) {
					Block(
#line  892 "cs.ATG" 
out stmt);
				} else if (la.kind == 11) {
					lexer.NextToken();
				} else SynErr(168);

#line  894 "cs.ATG" 
				List<ParameterDeclarationExpression> parameters = new List<ParameterDeclarationExpression>();
				parameters.Add(new ParameterDeclarationExpression(firstType, firstName));
				if (secondType != null) {
					parameters.Add(new ParameterDeclarationExpression(secondType, secondName));
				}
				OperatorDeclaration operatorDeclaration = new OperatorDeclaration {
					Modifier = m.Modifier,
					Attributes = attributes,
					Parameters = parameters,
					TypeReference = type,
					OverloadableOperator = op,
					Body = (BlockStatement)stmt,
					StartLocation = m.GetDeclarationLocation(startPos),
					EndLocation = endPos
				};
				SetParent(parameters, operatorDeclaration);
				compilationUnit.AddChild(operatorDeclaration);
				
			} else if (
#line  914 "cs.ATG" 
IsVarDecl()) {

#line  915 "cs.ATG" 
				m.Check(Modifiers.Fields);
				FieldDeclaration fd = new FieldDeclaration(attributes, type, m.Modifier);
				fd.StartLocation = m.GetDeclarationLocation(startPos); 
				
				if (
#line  919 "cs.ATG" 
m.Contains(Modifiers.Fixed)) {
					VariableDeclarator(
#line  920 "cs.ATG" 
fd);
					Expect(18);
					Expr(
#line  922 "cs.ATG" 
out expr);

#line  922 "cs.ATG" 
					if (fd.Fields.Count > 0)
					fd.Fields[fd.Fields.Count-1].FixedArrayInitialization = expr; 
					Expect(19);
					while (la.kind == 14) {
						lexer.NextToken();
						VariableDeclarator(
#line  926 "cs.ATG" 
fd);
						Expect(18);
						Expr(
#line  928 "cs.ATG" 
out expr);

#line  928 "cs.ATG" 
						if (fd.Fields.Count > 0)
						fd.Fields[fd.Fields.Count-1].FixedArrayInitialization = expr; 
						Expect(19);
					}
				} else if (StartOf(19)) {
					VariableDeclarator(
#line  933 "cs.ATG" 
fd);
					while (la.kind == 14) {
						lexer.NextToken();
						VariableDeclarator(
#line  934 "cs.ATG" 
fd);
					}
				} else SynErr(169);
				Expect(11);

#line  936 "cs.ATG" 
				fd.EndLocation = t.EndLocation; compilationUnit.AddChild(fd); 
			} else if (la.kind == 111) {

#line  939 "cs.ATG" 
				m.Check(Modifiers.Indexers); 
				lexer.NextToken();
				Expect(18);
				FormalParameterList(
#line  940 "cs.ATG" 
p);
				Expect(19);

#line  940 "cs.ATG" 
				Location endLocation = t.EndLocation; 
				Expect(16);

#line  941 "cs.ATG" 
				IndexerDeclaration indexer = new IndexerDeclaration(type, p, m.Modifier, attributes);
				indexer.StartLocation = startPos;
				indexer.EndLocation   = endLocation;
				indexer.BodyStart     = t.Location;
				PropertyGetRegion getRegion;
				PropertySetRegion setRegion;
				
				AccessorDecls(
#line  948 "cs.ATG" 
out getRegion, out setRegion);
				Expect(17);

#line  949 "cs.ATG" 
				indexer.BodyEnd    = t.EndLocation;
				indexer.GetRegion = getRegion;
				indexer.SetRegion = setRegion;
				compilationUnit.AddChild(indexer);
				
			} else if (
#line  954 "cs.ATG" 
IsIdentifierToken(la)) {
				if (
#line  955 "cs.ATG" 
IsExplicitInterfaceImplementation()) {
					TypeName(
#line  956 "cs.ATG" 
out explicitInterface, false);

#line  957 "cs.ATG" 
					if (la.kind != Tokens.Dot || Peek(1).kind != Tokens.This) {
					qualident = TypeReference.StripLastIdentifierFromType(ref explicitInterface);
					 } 
				} else if (StartOf(19)) {
					Identifier();

#line  960 "cs.ATG" 
					qualident = t.val; 
				} else SynErr(170);

#line  962 "cs.ATG" 
				Location qualIdentEndLocation = t.EndLocation; 
				if (la.kind == 16 || la.kind == 20 || la.kind == 23) {
					if (la.kind == 20 || la.kind == 23) {

#line  966 "cs.ATG" 
						m.Check(Modifiers.PropertysEventsMethods); 
						if (la.kind == 23) {
							TypeParameterList(
#line  968 "cs.ATG" 
templates);
						}
						Expect(20);
						if (la.kind == 111) {
							lexer.NextToken();

#line  970 "cs.ATG" 
							isExtensionMethod = true; 
						}
						if (StartOf(11)) {
							FormalParameterList(
#line  971 "cs.ATG" 
p);
						}
						Expect(21);

#line  973 "cs.ATG" 
						MethodDeclaration methodDeclaration = new MethodDeclaration {
						Name = qualident,
						Modifier = m.Modifier,
						TypeReference = type,
						Parameters = p, 
						Attributes = attributes
						};
						if (explicitInterface != null)
							methodDeclaration.InterfaceImplementations.Add(new InterfaceImplementation(explicitInterface, qualident));
						methodDeclaration.StartLocation = m.GetDeclarationLocation(startPos);
						methodDeclaration.EndLocation   = t.EndLocation;
						methodDeclaration.IsExtensionMethod = isExtensionMethod;
						methodDeclaration.Templates = templates;
						compilationUnit.AddChild(methodDeclaration);
						                                      
						while (la.kind == 127) {
							TypeParameterConstraintsClause(
#line  988 "cs.ATG" 
templates);
						}
						if (la.kind == 16) {
							Block(
#line  989 "cs.ATG" 
out stmt);
						} else if (la.kind == 11) {
							lexer.NextToken();
						} else SynErr(171);

#line  989 "cs.ATG" 
						methodDeclaration.Body  = (BlockStatement)stmt; 
					} else {
						lexer.NextToken();

#line  992 "cs.ATG" 
						PropertyDeclaration pDecl = new PropertyDeclaration(qualident, type, m.Modifier, attributes); 
						if (explicitInterface != null)
						pDecl.InterfaceImplementations.Add(new InterfaceImplementation(explicitInterface, qualident));
						      pDecl.StartLocation = m.GetDeclarationLocation(startPos);
						      pDecl.EndLocation   = qualIdentEndLocation;
						      pDecl.BodyStart   = t.Location;
						      PropertyGetRegion getRegion;
						      PropertySetRegion setRegion;
						   
						AccessorDecls(
#line  1001 "cs.ATG" 
out getRegion, out setRegion);
						Expect(17);

#line  1003 "cs.ATG" 
						pDecl.GetRegion = getRegion;
						pDecl.SetRegion = setRegion;
						pDecl.BodyEnd = t.EndLocation;
						compilationUnit.AddChild(pDecl);
						
					}
				} else if (la.kind == 15) {

#line  1011 "cs.ATG" 
					m.Check(Modifiers.Indexers); 
					lexer.NextToken();
					Expect(111);
					Expect(18);
					FormalParameterList(
#line  1012 "cs.ATG" 
p);
					Expect(19);

#line  1013 "cs.ATG" 
					IndexerDeclaration indexer = new IndexerDeclaration(type, p, m.Modifier, attributes);
					indexer.StartLocation = m.GetDeclarationLocation(startPos);
					indexer.EndLocation   = t.EndLocation;
					if (explicitInterface != null)
					indexer.InterfaceImplementations.Add(new InterfaceImplementation(explicitInterface, "this"));
					      PropertyGetRegion getRegion;
					      PropertySetRegion setRegion;
					    
					Expect(16);

#line  1021 "cs.ATG" 
					Location bodyStart = t.Location; 
					AccessorDecls(
#line  1022 "cs.ATG" 
out getRegion, out setRegion);
					Expect(17);

#line  1023 "cs.ATG" 
					indexer.BodyStart = bodyStart;
					indexer.BodyEnd   = t.EndLocation;
					indexer.GetRegion = getRegion;
					indexer.SetRegion = setRegion;
					compilationUnit.AddChild(indexer);
					
				} else SynErr(172);
			} else SynErr(173);
		} else SynErr(174);
	}

	void InterfaceMemberDecl() {

#line  1050 "cs.ATG" 
		TypeReference type;
		
		AttributeSection section;
		Modifiers mod = Modifiers.None;
		List<AttributeSection> attributes = new List<AttributeSection>();
		List<ParameterDeclarationExpression> parameters = new List<ParameterDeclarationExpression>();
		string name;
		PropertyGetRegion getBlock;
		PropertySetRegion setBlock;
		Location startLocation = new Location(-1, -1);
		List<TemplateDefinition> templates = new List<TemplateDefinition>();
		
		while (la.kind == 18) {
			AttributeSection(
#line  1063 "cs.ATG" 
out section);

#line  1063 "cs.ATG" 
			attributes.Add(section); 
		}
		if (la.kind == 89) {
			lexer.NextToken();

#line  1064 "cs.ATG" 
			mod = Modifiers.New; startLocation = t.Location; 
		}
		if (
#line  1067 "cs.ATG" 
NotVoidPointer()) {
			Expect(123);

#line  1067 "cs.ATG" 
			if (startLocation.X == -1) startLocation = t.Location; 
			Identifier();

#line  1068 "cs.ATG" 
			name = t.val; 
			if (la.kind == 23) {
				TypeParameterList(
#line  1069 "cs.ATG" 
templates);
			}
			Expect(20);
			if (StartOf(11)) {
				FormalParameterList(
#line  1070 "cs.ATG" 
parameters);
			}
			Expect(21);
			while (la.kind == 127) {
				TypeParameterConstraintsClause(
#line  1071 "cs.ATG" 
templates);
			}
			Expect(11);

#line  1073 "cs.ATG" 
			MethodDeclaration md = new MethodDeclaration {
			Name = name, Modifier = mod, TypeReference = new TypeReference("System.Void", true), 
			Parameters = parameters, Attributes = attributes, Templates = templates,
			StartLocation = startLocation, EndLocation = t.EndLocation
			};
			compilationUnit.AddChild(md);
			
		} else if (StartOf(23)) {
			if (StartOf(10)) {
				Type(
#line  1081 "cs.ATG" 
out type);

#line  1081 "cs.ATG" 
				if (startLocation.X == -1) startLocation = t.Location; 
				if (StartOf(19)) {
					Identifier();

#line  1083 "cs.ATG" 
					name = t.val; Location qualIdentEndLocation = t.EndLocation; 
					if (la.kind == 20 || la.kind == 23) {
						if (la.kind == 23) {
							TypeParameterList(
#line  1087 "cs.ATG" 
templates);
						}
						Expect(20);
						if (StartOf(11)) {
							FormalParameterList(
#line  1088 "cs.ATG" 
parameters);
						}
						Expect(21);
						while (la.kind == 127) {
							TypeParameterConstraintsClause(
#line  1090 "cs.ATG" 
templates);
						}
						Expect(11);

#line  1091 "cs.ATG" 
						MethodDeclaration md = new MethodDeclaration {
						Name = name, Modifier = mod, TypeReference = type,
						Parameters = parameters, Attributes = attributes, Templates = templates,
						StartLocation = startLocation, EndLocation = t.EndLocation
						};
						compilationUnit.AddChild(md);
						
					} else if (la.kind == 16) {

#line  1099 "cs.ATG" 
						PropertyDeclaration pd = new PropertyDeclaration(name, type, mod, attributes); compilationUnit.AddChild(pd); 
						lexer.NextToken();

#line  1100 "cs.ATG" 
						Location bodyStart = t.Location;
						InterfaceAccessors(
#line  1100 "cs.ATG" 
out getBlock, out setBlock);
						Expect(17);

#line  1100 "cs.ATG" 
						pd.GetRegion = getBlock; pd.SetRegion = setBlock; pd.StartLocation = startLocation; pd.EndLocation = qualIdentEndLocation; pd.BodyStart = bodyStart; pd.BodyEnd = t.EndLocation; 
					} else SynErr(175);
				} else if (la.kind == 111) {
					lexer.NextToken();
					Expect(18);
					FormalParameterList(
#line  1103 "cs.ATG" 
parameters);
					Expect(19);

#line  1103 "cs.ATG" 
					Location bracketEndLocation = t.EndLocation; 

#line  1103 "cs.ATG" 
					IndexerDeclaration id = new IndexerDeclaration(type, parameters, mod, attributes); compilationUnit.AddChild(id); 
					Expect(16);

#line  1104 "cs.ATG" 
					Location bodyStart = t.Location;
					InterfaceAccessors(
#line  1104 "cs.ATG" 
out getBlock, out setBlock);
					Expect(17);

#line  1104 "cs.ATG" 
					id.GetRegion = getBlock; id.SetRegion = setBlock; id.StartLocation = startLocation;  id.EndLocation = bracketEndLocation; id.BodyStart = bodyStart; id.BodyEnd = t.EndLocation;
				} else SynErr(176);
			} else {
				lexer.NextToken();

#line  1107 "cs.ATG" 
				if (startLocation.X == -1) startLocation = t.Location; 
				Type(
#line  1108 "cs.ATG" 
out type);
				Identifier();

#line  1109 "cs.ATG" 
				EventDeclaration ed = new EventDeclaration {
				TypeReference = type, Name = t.val, Modifier = mod, Attributes = attributes
				};
				compilationUnit.AddChild(ed);
				
				Expect(11);

#line  1114 "cs.ATG" 
				ed.StartLocation = startLocation; ed.EndLocation = t.EndLocation; 
			}
		} else SynErr(177);
	}

	void EnumMemberDecl(
#line  1119 "cs.ATG" 
out FieldDeclaration f) {

#line  1121 "cs.ATG" 
		Expression expr = null;
		List<AttributeSection> attributes = new List<AttributeSection>();
		AttributeSection section = null;
		VariableDeclaration varDecl = null;
		
		while (la.kind == 18) {
			AttributeSection(
#line  1127 "cs.ATG" 
out section);

#line  1127 "cs.ATG" 
			attributes.Add(section); 
		}
		Identifier();

#line  1128 "cs.ATG" 
		f = new FieldDeclaration(attributes);
		varDecl         = new VariableDeclaration(t.val);
		f.Fields.Add(varDecl);
		f.StartLocation = t.Location;
		
		if (la.kind == 3) {
			lexer.NextToken();
			Expr(
#line  1133 "cs.ATG" 
out expr);

#line  1133 "cs.ATG" 
			varDecl.Initializer = expr; 
		}
	}

	void TypeWithRestriction(
#line  556 "cs.ATG" 
out TypeReference type, bool allowNullable, bool canBeUnbound) {

#line  558 "cs.ATG" 
		string name;
		int pointer = 0;
		type = null;
		
		if (StartOf(4)) {
			ClassType(
#line  563 "cs.ATG" 
out type, canBeUnbound);
		} else if (StartOf(5)) {
			SimpleType(
#line  564 "cs.ATG" 
out name);

#line  564 "cs.ATG" 
			type = new TypeReference(name, true); 
		} else if (la.kind == 123) {
			lexer.NextToken();
			Expect(6);

#line  565 "cs.ATG" 
			pointer = 1; type = new TypeReference("System.Void", true); 
		} else SynErr(178);

#line  566 "cs.ATG" 
		List<int> r = new List<int>(); 
		if (
#line  568 "cs.ATG" 
allowNullable && la.kind == Tokens.Question) {
			NullableQuestionMark(
#line  568 "cs.ATG" 
ref type);
		}
		while (
#line  570 "cs.ATG" 
IsPointerOrDims()) {

#line  570 "cs.ATG" 
			int i = 0; 
			if (la.kind == 6) {
				lexer.NextToken();

#line  571 "cs.ATG" 
				++pointer; 
			} else if (la.kind == 18) {
				lexer.NextToken();
				while (la.kind == 14) {
					lexer.NextToken();

#line  572 "cs.ATG" 
					++i; 
				}
				Expect(19);

#line  572 "cs.ATG" 
				r.Add(i); 
			} else SynErr(179);
		}

#line  575 "cs.ATG" 
		if (type != null) {
		type.RankSpecifier = r.ToArray();
		type.PointerNestingLevel = pointer;
		  }
		
	}

	void SimpleType(
#line  603 "cs.ATG" 
out string name) {

#line  604 "cs.ATG" 
		name = String.Empty; 
		if (StartOf(24)) {
			IntegralType(
#line  606 "cs.ATG" 
out name);
		} else if (la.kind == 75) {
			lexer.NextToken();

#line  607 "cs.ATG" 
			name = "System.Single"; 
		} else if (la.kind == 66) {
			lexer.NextToken();

#line  608 "cs.ATG" 
			name = "System.Double"; 
		} else if (la.kind == 62) {
			lexer.NextToken();

#line  609 "cs.ATG" 
			name = "System.Decimal"; 
		} else if (la.kind == 52) {
			lexer.NextToken();

#line  610 "cs.ATG" 
			name = "System.Boolean"; 
		} else SynErr(180);
	}

	void NullableQuestionMark(
#line  2278 "cs.ATG" 
ref TypeReference typeRef) {

#line  2279 "cs.ATG" 
		List<TypeReference> typeArguments = new List<TypeReference>(1); 
		Expect(12);

#line  2283 "cs.ATG" 
		if (typeRef != null) typeArguments.Add(typeRef);
		typeRef = new TypeReference("System.Nullable", typeArguments) { IsKeyword = true };
		
	}

	void FixedParameter(
#line  640 "cs.ATG" 
out ParameterDeclarationExpression p) {

#line  642 "cs.ATG" 
		TypeReference type;
		ParameterModifiers mod = ParameterModifiers.In;
		Location start = t.Location;
		
		if (la.kind == 93 || la.kind == 100) {
			if (la.kind == 100) {
				lexer.NextToken();

#line  648 "cs.ATG" 
				mod = ParameterModifiers.Ref; 
			} else {
				lexer.NextToken();

#line  649 "cs.ATG" 
				mod = ParameterModifiers.Out; 
			}
		}
		Type(
#line  651 "cs.ATG" 
out type);
		Identifier();

#line  651 "cs.ATG" 
		p = new ParameterDeclarationExpression(type, t.val, mod); p.StartLocation = start; p.EndLocation = t.Location; 
	}

	void ParameterArray(
#line  654 "cs.ATG" 
out ParameterDeclarationExpression p) {

#line  655 "cs.ATG" 
		TypeReference type; 
		Expect(95);
		Type(
#line  657 "cs.ATG" 
out type);
		Identifier();

#line  657 "cs.ATG" 
		p = new ParameterDeclarationExpression(type, t.val, ParameterModifiers.Params); 
	}

	void AccessorModifiers(
#line  660 "cs.ATG" 
out ModifierList m) {

#line  661 "cs.ATG" 
		m = new ModifierList(); 
		if (la.kind == 96) {
			lexer.NextToken();

#line  663 "cs.ATG" 
			m.Add(Modifiers.Private, t.Location); 
		} else if (la.kind == 97) {
			lexer.NextToken();

#line  664 "cs.ATG" 
			m.Add(Modifiers.Protected, t.Location); 
			if (la.kind == 84) {
				lexer.NextToken();

#line  665 "cs.ATG" 
				m.Add(Modifiers.Internal, t.Location); 
			}
		} else if (la.kind == 84) {
			lexer.NextToken();

#line  666 "cs.ATG" 
			m.Add(Modifiers.Internal, t.Location); 
			if (la.kind == 97) {
				lexer.NextToken();

#line  667 "cs.ATG" 
				m.Add(Modifiers.Protected, t.Location); 
			}
		} else SynErr(181);
	}

	void Block(
#line  1253 "cs.ATG" 
out Statement stmt) {
		Expect(16);

#line  1255 "cs.ATG" 
		BlockStatement blockStmt = new BlockStatement();
		blockStmt.StartLocation = t.Location;
		compilationUnit.BlockStart(blockStmt);
		if (!ParseMethodBodies) lexer.SkipCurrentBlock(0);
		
		while (StartOf(25)) {
			Statement();
		}
		while (!(la.kind == 0 || la.kind == 17)) {SynErr(182); lexer.NextToken(); }
		Expect(17);

#line  1263 "cs.ATG" 
		stmt = blockStmt;
		blockStmt.EndLocation = t.EndLocation;
		compilationUnit.BlockEnd();
		
	}

	void EventAccessorDecls(
#line  1190 "cs.ATG" 
out EventAddRegion addBlock, out EventRemoveRegion removeBlock) {

#line  1191 "cs.ATG" 
		AttributeSection section;
		List<AttributeSection> attributes = new List<AttributeSection>();
		Statement stmt;
		addBlock = null;
		removeBlock = null;
		
		while (la.kind == 18) {
			AttributeSection(
#line  1198 "cs.ATG" 
out section);

#line  1198 "cs.ATG" 
			attributes.Add(section); 
		}
		if (la.kind == 130) {

#line  1200 "cs.ATG" 
			addBlock = new EventAddRegion(attributes); 
			AddAccessorDecl(
#line  1201 "cs.ATG" 
out stmt);

#line  1201 "cs.ATG" 
			attributes = new List<AttributeSection>(); addBlock.Block = (BlockStatement)stmt; 
			while (la.kind == 18) {
				AttributeSection(
#line  1202 "cs.ATG" 
out section);

#line  1202 "cs.ATG" 
				attributes.Add(section); 
			}
			RemoveAccessorDecl(
#line  1203 "cs.ATG" 
out stmt);

#line  1203 "cs.ATG" 
			removeBlock = new EventRemoveRegion(attributes); removeBlock.Block = (BlockStatement)stmt; 
		} else if (la.kind == 131) {
			RemoveAccessorDecl(
#line  1205 "cs.ATG" 
out stmt);

#line  1205 "cs.ATG" 
			removeBlock = new EventRemoveRegion(attributes); removeBlock.Block = (BlockStatement)stmt; attributes = new List<AttributeSection>(); 
			while (la.kind == 18) {
				AttributeSection(
#line  1206 "cs.ATG" 
out section);

#line  1206 "cs.ATG" 
				attributes.Add(section); 
			}
			AddAccessorDecl(
#line  1207 "cs.ATG" 
out stmt);

#line  1207 "cs.ATG" 
			addBlock = new EventAddRegion(attributes); addBlock.Block = (BlockStatement)stmt; 
		} else SynErr(183);
	}

	void ConstructorInitializer(
#line  1283 "cs.ATG" 
out ConstructorInitializer ci) {

#line  1284 "cs.ATG" 
		Expression expr; ci = new ConstructorInitializer(); 
		Expect(9);
		if (la.kind == 51) {
			lexer.NextToken();

#line  1288 "cs.ATG" 
			ci.ConstructorInitializerType = ConstructorInitializerType.Base; 
		} else if (la.kind == 111) {
			lexer.NextToken();

#line  1289 "cs.ATG" 
			ci.ConstructorInitializerType = ConstructorInitializerType.This; 
		} else SynErr(184);
		Expect(20);
		if (StartOf(26)) {
			Argument(
#line  1292 "cs.ATG" 
out expr);

#line  1292 "cs.ATG" 
			SafeAdd(ci, ci.Arguments, expr); 
			while (la.kind == 14) {
				lexer.NextToken();
				Argument(
#line  1293 "cs.ATG" 
out expr);

#line  1293 "cs.ATG" 
				SafeAdd(ci, ci.Arguments, expr); 
			}
		}
		Expect(21);
	}

	void OverloadableOperator(
#line  1306 "cs.ATG" 
out OverloadableOperatorType op) {

#line  1307 "cs.ATG" 
		op = OverloadableOperatorType.None; 
		switch (la.kind) {
		case 4: {
			lexer.NextToken();

#line  1309 "cs.ATG" 
			op = OverloadableOperatorType.Add; 
			break;
		}
		case 5: {
			lexer.NextToken();

#line  1310 "cs.ATG" 
			op = OverloadableOperatorType.Subtract; 
			break;
		}
		case 24: {
			lexer.NextToken();

#line  1312 "cs.ATG" 
			op = OverloadableOperatorType.Not; 
			break;
		}
		case 27: {
			lexer.NextToken();

#line  1313 "cs.ATG" 
			op = OverloadableOperatorType.BitNot; 
			break;
		}
		case 31: {
			lexer.NextToken();

#line  1315 "cs.ATG" 
			op = OverloadableOperatorType.Increment; 
			break;
		}
		case 32: {
			lexer.NextToken();

#line  1316 "cs.ATG" 
			op = OverloadableOperatorType.Decrement; 
			break;
		}
		case 113: {
			lexer.NextToken();

#line  1318 "cs.ATG" 
			op = OverloadableOperatorType.IsTrue; 
			break;
		}
		case 72: {
			lexer.NextToken();

#line  1319 "cs.ATG" 
			op = OverloadableOperatorType.IsFalse; 
			break;
		}
		case 6: {
			lexer.NextToken();

#line  1321 "cs.ATG" 
			op = OverloadableOperatorType.Multiply; 
			break;
		}
		case 7: {
			lexer.NextToken();

#line  1322 "cs.ATG" 
			op = OverloadableOperatorType.Divide; 
			break;
		}
		case 8: {
			lexer.NextToken();

#line  1323 "cs.ATG" 
			op = OverloadableOperatorType.Modulus; 
			break;
		}
		case 28: {
			lexer.NextToken();

#line  1325 "cs.ATG" 
			op = OverloadableOperatorType.BitwiseAnd; 
			break;
		}
		case 29: {
			lexer.NextToken();

#line  1326 "cs.ATG" 
			op = OverloadableOperatorType.BitwiseOr; 
			break;
		}
		case 30: {
			lexer.NextToken();

#line  1327 "cs.ATG" 
			op = OverloadableOperatorType.ExclusiveOr; 
			break;
		}
		case 37: {
			lexer.NextToken();

#line  1329 "cs.ATG" 
			op = OverloadableOperatorType.ShiftLeft; 
			break;
		}
		case 33: {
			lexer.NextToken();

#line  1330 "cs.ATG" 
			op = OverloadableOperatorType.Equality; 
			break;
		}
		case 34: {
			lexer.NextToken();

#line  1331 "cs.ATG" 
			op = OverloadableOperatorType.InEquality; 
			break;
		}
		case 23: {
			lexer.NextToken();

#line  1332 "cs.ATG" 
			op = OverloadableOperatorType.LessThan; 
			break;
		}
		case 35: {
			lexer.NextToken();

#line  1333 "cs.ATG" 
			op = OverloadableOperatorType.GreaterThanOrEqual; 
			break;
		}
		case 36: {
			lexer.NextToken();

#line  1334 "cs.ATG" 
			op = OverloadableOperatorType.LessThanOrEqual; 
			break;
		}
		case 22: {
			lexer.NextToken();

#line  1335 "cs.ATG" 
			op = OverloadableOperatorType.GreaterThan; 
			if (la.kind == 22) {
				lexer.NextToken();

#line  1335 "cs.ATG" 
				op = OverloadableOperatorType.ShiftRight; 
			}
			break;
		}
		default: SynErr(185); break;
		}
	}

	void VariableDeclarator(
#line  1245 "cs.ATG" 
FieldDeclaration parentFieldDeclaration) {

#line  1246 "cs.ATG" 
		Expression expr = null; 
		Identifier();

#line  1248 "cs.ATG" 
		VariableDeclaration f = new VariableDeclaration(t.val); 
		if (la.kind == 3) {
			lexer.NextToken();
			VariableInitializer(
#line  1249 "cs.ATG" 
out expr);

#line  1249 "cs.ATG" 
			f.Initializer = expr; 
		}

#line  1250 "cs.ATG" 
		SafeAdd(parentFieldDeclaration, parentFieldDeclaration.Fields, f); 
	}

	void AccessorDecls(
#line  1137 "cs.ATG" 
out PropertyGetRegion getBlock, out PropertySetRegion setBlock) {

#line  1139 "cs.ATG" 
		List<AttributeSection> attributes = new List<AttributeSection>(); 
		AttributeSection section;
		getBlock = null;
		setBlock = null; 
		ModifierList modifiers = null;
		
		while (la.kind == 18) {
			AttributeSection(
#line  1146 "cs.ATG" 
out section);

#line  1146 "cs.ATG" 
			attributes.Add(section); 
		}
		if (la.kind == 84 || la.kind == 96 || la.kind == 97) {
			AccessorModifiers(
#line  1147 "cs.ATG" 
out modifiers);
		}
		if (la.kind == 128) {
			GetAccessorDecl(
#line  1149 "cs.ATG" 
out getBlock, attributes);

#line  1150 "cs.ATG" 
			if (modifiers != null) {getBlock.Modifier = modifiers.Modifier; } 
			if (StartOf(27)) {

#line  1151 "cs.ATG" 
				attributes = new List<AttributeSection>(); modifiers = null; 
				while (la.kind == 18) {
					AttributeSection(
#line  1152 "cs.ATG" 
out section);

#line  1152 "cs.ATG" 
					attributes.Add(section); 
				}
				if (la.kind == 84 || la.kind == 96 || la.kind == 97) {
					AccessorModifiers(
#line  1153 "cs.ATG" 
out modifiers);
				}
				SetAccessorDecl(
#line  1154 "cs.ATG" 
out setBlock, attributes);

#line  1155 "cs.ATG" 
				if (modifiers != null) {setBlock.Modifier = modifiers.Modifier; } 
			}
		} else if (la.kind == 129) {
			SetAccessorDecl(
#line  1158 "cs.ATG" 
out setBlock, attributes);

#line  1159 "cs.ATG" 
			if (modifiers != null) {setBlock.Modifier = modifiers.Modifier; } 
			if (StartOf(28)) {

#line  1160 "cs.ATG" 
				attributes = new List<AttributeSection>(); modifiers = null; 
				while (la.kind == 18) {
					AttributeSection(
#line  1161 "cs.ATG" 
out section);

#line  1161 "cs.ATG" 
					attributes.Add(section); 
				}
				if (la.kind == 84 || la.kind == 96 || la.kind == 97) {
					AccessorModifiers(
#line  1162 "cs.ATG" 
out modifiers);
				}
				GetAccessorDecl(
#line  1163 "cs.ATG" 
out getBlock, attributes);

#line  1164 "cs.ATG" 
				if (modifiers != null) {getBlock.Modifier = modifiers.Modifier; } 
			}
		} else if (StartOf(19)) {
			Identifier();

#line  1166 "cs.ATG" 
			Error("get or set accessor declaration expected"); 
		} else SynErr(186);
	}

	void InterfaceAccessors(
#line  1211 "cs.ATG" 
out PropertyGetRegion getBlock, out PropertySetRegion setBlock) {

#line  1213 "cs.ATG" 
		AttributeSection section;
		List<AttributeSection> attributes = new List<AttributeSection>();
		getBlock = null; setBlock = null;
		PropertyGetSetRegion lastBlock = null;
		
		while (la.kind == 18) {
			AttributeSection(
#line  1219 "cs.ATG" 
out section);

#line  1219 "cs.ATG" 
			attributes.Add(section); 
		}

#line  1220 "cs.ATG" 
		Location startLocation = la.Location; 
		if (la.kind == 128) {
			lexer.NextToken();

#line  1222 "cs.ATG" 
			getBlock = new PropertyGetRegion(null, attributes); 
		} else if (la.kind == 129) {
			lexer.NextToken();

#line  1223 "cs.ATG" 
			setBlock = new PropertySetRegion(null, attributes); 
		} else SynErr(187);
		Expect(11);

#line  1226 "cs.ATG" 
		if (getBlock != null) { getBlock.StartLocation = startLocation; getBlock.EndLocation = t.EndLocation; }
		if (setBlock != null) { setBlock.StartLocation = startLocation; setBlock.EndLocation = t.EndLocation; }
		attributes = new List<AttributeSection>(); 
		if (la.kind == 18 || la.kind == 128 || la.kind == 129) {
			while (la.kind == 18) {
				AttributeSection(
#line  1230 "cs.ATG" 
out section);

#line  1230 "cs.ATG" 
				attributes.Add(section); 
			}

#line  1231 "cs.ATG" 
			startLocation = la.Location; 
			if (la.kind == 128) {
				lexer.NextToken();

#line  1233 "cs.ATG" 
				if (getBlock != null) Error("get already declared");
				                 else { getBlock = new PropertyGetRegion(null, attributes); lastBlock = getBlock; }
				              
			} else if (la.kind == 129) {
				lexer.NextToken();

#line  1236 "cs.ATG" 
				if (setBlock != null) Error("set already declared");
				                 else { setBlock = new PropertySetRegion(null, attributes); lastBlock = setBlock; }
				              
			} else SynErr(188);
			Expect(11);

#line  1241 "cs.ATG" 
			if (lastBlock != null) { lastBlock.StartLocation = startLocation; lastBlock.EndLocation = t.EndLocation; } 
		}
	}

	void GetAccessorDecl(
#line  1170 "cs.ATG" 
out PropertyGetRegion getBlock, List<AttributeSection> attributes) {

#line  1171 "cs.ATG" 
		Statement stmt = null; 
		Expect(128);

#line  1174 "cs.ATG" 
		Location startLocation = t.Location; 
		if (la.kind == 16) {
			Block(
#line  1175 "cs.ATG" 
out stmt);
		} else if (la.kind == 11) {
			lexer.NextToken();
		} else SynErr(189);

#line  1176 "cs.ATG" 
		getBlock = new PropertyGetRegion((BlockStatement)stmt, attributes); 

#line  1177 "cs.ATG" 
		getBlock.StartLocation = startLocation; getBlock.EndLocation = t.EndLocation; 
	}

	void SetAccessorDecl(
#line  1180 "cs.ATG" 
out PropertySetRegion setBlock, List<AttributeSection> attributes) {

#line  1181 "cs.ATG" 
		Statement stmt = null; 
		Expect(129);

#line  1184 "cs.ATG" 
		Location startLocation = t.Location; 
		if (la.kind == 16) {
			Block(
#line  1185 "cs.ATG" 
out stmt);
		} else if (la.kind == 11) {
			lexer.NextToken();
		} else SynErr(190);

#line  1186 "cs.ATG" 
		setBlock = new PropertySetRegion((BlockStatement)stmt, attributes); 

#line  1187 "cs.ATG" 
		setBlock.StartLocation = startLocation; setBlock.EndLocation = t.EndLocation; 
	}

	void AddAccessorDecl(
#line  1269 "cs.ATG" 
out Statement stmt) {

#line  1270 "cs.ATG" 
		stmt = null;
		Expect(130);
		Block(
#line  1273 "cs.ATG" 
out stmt);
	}

	void RemoveAccessorDecl(
#line  1276 "cs.ATG" 
out Statement stmt) {

#line  1277 "cs.ATG" 
		stmt = null;
		Expect(131);
		Block(
#line  1280 "cs.ATG" 
out stmt);
	}

	void VariableInitializer(
#line  1298 "cs.ATG" 
out Expression initializerExpression) {

#line  1299 "cs.ATG" 
		TypeReference type = null; Expression expr = null; initializerExpression = null; 
		if (StartOf(6)) {
			Expr(
#line  1301 "cs.ATG" 
out initializerExpression);
		} else if (la.kind == 16) {
			CollectionInitializer(
#line  1302 "cs.ATG" 
out initializerExpression);
		} else if (la.kind == 106) {
			lexer.NextToken();
			Type(
#line  1303 "cs.ATG" 
out type);
			Expect(18);
			Expr(
#line  1303 "cs.ATG" 
out expr);
			Expect(19);

#line  1303 "cs.ATG" 
			initializerExpression = new StackAllocExpression(type, expr); 
		} else SynErr(191);
	}

	void Statement() {

#line  1444 "cs.ATG" 
		TypeReference type;
		Expression expr;
		Statement stmt = null;
		Location startPos = la.Location;
		
		while (!(StartOf(29))) {SynErr(192); lexer.NextToken(); }
		if (
#line  1453 "cs.ATG" 
IsLabel()) {
			Identifier();

#line  1453 "cs.ATG" 
			compilationUnit.AddChild(new LabelStatement(t.val)); 
			Expect(9);
			Statement();
		} else if (la.kind == 60) {
			lexer.NextToken();
			Type(
#line  1456 "cs.ATG" 
out type);

#line  1456 "cs.ATG" 
			LocalVariableDeclaration var = new LocalVariableDeclaration(type, Modifiers.Const); string ident = null; var.StartLocation = t.Location; 
			Identifier();

#line  1457 "cs.ATG" 
			ident = t.val; 
			Expect(3);
			Expr(
#line  1458 "cs.ATG" 
out expr);

#line  1458 "cs.ATG" 
			SafeAdd(var, var.Variables, new VariableDeclaration(ident, expr)); 
			while (la.kind == 14) {
				lexer.NextToken();
				Identifier();

#line  1459 "cs.ATG" 
				ident = t.val; 
				Expect(3);
				Expr(
#line  1459 "cs.ATG" 
out expr);

#line  1459 "cs.ATG" 
				SafeAdd(var, var.Variables, new VariableDeclaration(ident, expr)); 
			}
			Expect(11);

#line  1460 "cs.ATG" 
			compilationUnit.AddChild(var); 
		} else if (
#line  1463 "cs.ATG" 
IsLocalVarDecl()) {
			LocalVariableDecl(
#line  1463 "cs.ATG" 
out stmt);
			Expect(11);

#line  1463 "cs.ATG" 
			compilationUnit.AddChild(stmt); 
		} else if (StartOf(30)) {
			EmbeddedStatement(
#line  1465 "cs.ATG" 
out stmt);

#line  1465 "cs.ATG" 
			compilationUnit.AddChild(stmt); 
		} else SynErr(193);

#line  1471 "cs.ATG" 
		if (stmt != null) {
		stmt.StartLocation = startPos;
		stmt.EndLocation = t.EndLocation;
		}
		
	}

	void Argument(
#line  1338 "cs.ATG" 
out Expression argumentexpr) {

#line  1340 "cs.ATG" 
		Expression expr;
		FieldDirection fd = FieldDirection.None;
		
		if (la.kind == 93 || la.kind == 100) {
			if (la.kind == 100) {
				lexer.NextToken();

#line  1345 "cs.ATG" 
				fd = FieldDirection.Ref; 
			} else {
				lexer.NextToken();

#line  1346 "cs.ATG" 
				fd = FieldDirection.Out; 
			}
		}
		Expr(
#line  1348 "cs.ATG" 
out expr);

#line  1349 "cs.ATG" 
		argumentexpr = fd != FieldDirection.None ? argumentexpr = new DirectionExpression(fd, expr) : expr; 
	}

	void CollectionInitializer(
#line  1369 "cs.ATG" 
out Expression outExpr) {

#line  1371 "cs.ATG" 
		Expression expr = null;
		CollectionInitializerExpression initializer = new CollectionInitializerExpression();
		
		Expect(16);

#line  1375 "cs.ATG" 
		initializer.StartLocation = t.Location; 
		if (StartOf(31)) {
			VariableInitializer(
#line  1376 "cs.ATG" 
out expr);

#line  1377 "cs.ATG" 
			SafeAdd(initializer, initializer.CreateExpressions, expr); 
			while (
#line  1378 "cs.ATG" 
NotFinalComma()) {
				Expect(14);
				VariableInitializer(
#line  1379 "cs.ATG" 
out expr);

#line  1380 "cs.ATG" 
				SafeAdd(initializer, initializer.CreateExpressions, expr); 
			}
			if (la.kind == 14) {
				lexer.NextToken();
			}
		}
		Expect(17);

#line  1384 "cs.ATG" 
		initializer.EndLocation = t.Location; outExpr = initializer; 
	}

	void AssignmentOperator(
#line  1352 "cs.ATG" 
out AssignmentOperatorType op) {

#line  1353 "cs.ATG" 
		op = AssignmentOperatorType.None; 
		if (la.kind == 3) {
			lexer.NextToken();

#line  1355 "cs.ATG" 
			op = AssignmentOperatorType.Assign; 
		} else if (la.kind == 38) {
			lexer.NextToken();

#line  1356 "cs.ATG" 
			op = AssignmentOperatorType.Add; 
		} else if (la.kind == 39) {
			lexer.NextToken();

#line  1357 "cs.ATG" 
			op = AssignmentOperatorType.Subtract; 
		} else if (la.kind == 40) {
			lexer.NextToken();

#line  1358 "cs.ATG" 
			op = AssignmentOperatorType.Multiply; 
		} else if (la.kind == 41) {
			lexer.NextToken();

#line  1359 "cs.ATG" 
			op = AssignmentOperatorType.Divide; 
		} else if (la.kind == 42) {
			lexer.NextToken();

#line  1360 "cs.ATG" 
			op = AssignmentOperatorType.Modulus; 
		} else if (la.kind == 43) {
			lexer.NextToken();

#line  1361 "cs.ATG" 
			op = AssignmentOperatorType.BitwiseAnd; 
		} else if (la.kind == 44) {
			lexer.NextToken();

#line  1362 "cs.ATG" 
			op = AssignmentOperatorType.BitwiseOr; 
		} else if (la.kind == 45) {
			lexer.NextToken();

#line  1363 "cs.ATG" 
			op = AssignmentOperatorType.ExclusiveOr; 
		} else if (la.kind == 46) {
			lexer.NextToken();

#line  1364 "cs.ATG" 
			op = AssignmentOperatorType.ShiftLeft; 
		} else if (
#line  1365 "cs.ATG" 
la.kind == Tokens.GreaterThan && Peek(1).kind == Tokens.GreaterEqual) {
			Expect(22);
			Expect(35);

#line  1366 "cs.ATG" 
			op = AssignmentOperatorType.ShiftRight; 
		} else SynErr(194);
	}

	void CollectionOrObjectInitializer(
#line  1387 "cs.ATG" 
out Expression outExpr) {

#line  1389 "cs.ATG" 
		Expression expr = null;
		CollectionInitializerExpression initializer = new CollectionInitializerExpression();
		
		Expect(16);

#line  1393 "cs.ATG" 
		initializer.StartLocation = t.Location; 
		if (StartOf(31)) {
			ObjectPropertyInitializerOrVariableInitializer(
#line  1394 "cs.ATG" 
out expr);

#line  1395 "cs.ATG" 
			SafeAdd(initializer, initializer.CreateExpressions, expr); 
			while (
#line  1396 "cs.ATG" 
NotFinalComma()) {
				Expect(14);
				ObjectPropertyInitializerOrVariableInitializer(
#line  1397 "cs.ATG" 
out expr);

#line  1398 "cs.ATG" 
				SafeAdd(initializer, initializer.CreateExpressions, expr); 
			}
			if (la.kind == 14) {
				lexer.NextToken();
			}
		}
		Expect(17);

#line  1402 "cs.ATG" 
		initializer.EndLocation = t.Location; outExpr = initializer; 
	}

	void ObjectPropertyInitializerOrVariableInitializer(
#line  1405 "cs.ATG" 
out Expression expr) {

#line  1406 "cs.ATG" 
		expr = null; 
		if (
#line  1408 "cs.ATG" 
IdentAndAsgn()) {
			Identifier();

#line  1410 "cs.ATG" 
			NamedArgumentExpression nae = new NamedArgumentExpression(t.val, null);
			nae.StartLocation = t.Location;
			Expression r = null; 
			Expect(3);
			if (la.kind == 16) {
				CollectionOrObjectInitializer(
#line  1414 "cs.ATG" 
out r);
			} else if (StartOf(31)) {
				VariableInitializer(
#line  1415 "cs.ATG" 
out r);
			} else SynErr(195);

#line  1416 "cs.ATG" 
			nae.Expression = r; nae.EndLocation = t.EndLocation; expr = nae; 
		} else if (StartOf(31)) {
			VariableInitializer(
#line  1418 "cs.ATG" 
out expr);
		} else SynErr(196);
	}

	void LocalVariableDecl(
#line  1422 "cs.ATG" 
out Statement stmt) {

#line  1424 "cs.ATG" 
		TypeReference type;
		VariableDeclaration      var = null;
		LocalVariableDeclaration localVariableDeclaration; 
		
		Type(
#line  1429 "cs.ATG" 
out type);

#line  1429 "cs.ATG" 
		localVariableDeclaration = new LocalVariableDeclaration(type); localVariableDeclaration.StartLocation = t.Location; 
		LocalVariableDeclarator(
#line  1430 "cs.ATG" 
out var);

#line  1430 "cs.ATG" 
		SafeAdd(localVariableDeclaration, localVariableDeclaration.Variables, var); 
		while (la.kind == 14) {
			lexer.NextToken();
			LocalVariableDeclarator(
#line  1431 "cs.ATG" 
out var);

#line  1431 "cs.ATG" 
			SafeAdd(localVariableDeclaration, localVariableDeclaration.Variables, var); 
		}

#line  1432 "cs.ATG" 
		stmt = localVariableDeclaration; 
	}

	void LocalVariableDeclarator(
#line  1435 "cs.ATG" 
out VariableDeclaration var) {

#line  1436 "cs.ATG" 
		Expression expr = null; 
		Identifier();

#line  1438 "cs.ATG" 
		var = new VariableDeclaration(t.val); 
		if (la.kind == 3) {
			lexer.NextToken();
			VariableInitializer(
#line  1439 "cs.ATG" 
out expr);

#line  1439 "cs.ATG" 
			var.Initializer = expr; 
		}
	}

	void EmbeddedStatement(
#line  1478 "cs.ATG" 
out Statement statement) {

#line  1480 "cs.ATG" 
		TypeReference type = null;
		Expression expr = null;
		Statement embeddedStatement = null;
		statement = null;
		

#line  1486 "cs.ATG" 
		Location startLocation = la.Location; 
		if (la.kind == 16) {
			Block(
#line  1488 "cs.ATG" 
out statement);
		} else if (la.kind == 11) {
			lexer.NextToken();

#line  1491 "cs.ATG" 
			statement = new EmptyStatement(); 
		} else if (
#line  1494 "cs.ATG" 
UnCheckedAndLBrace()) {

#line  1494 "cs.ATG" 
			Statement block; bool isChecked = true; 
			if (la.kind == 58) {
				lexer.NextToken();
			} else if (la.kind == 118) {
				lexer.NextToken();

#line  1495 "cs.ATG" 
				isChecked = false;
			} else SynErr(197);
			Block(
#line  1496 "cs.ATG" 
out block);

#line  1496 "cs.ATG" 
			statement = isChecked ? (Statement)new CheckedStatement(block) : (Statement)new UncheckedStatement(block); 
		} else if (la.kind == 79) {
			IfStatement(
#line  1499 "cs.ATG" 
out statement);
		} else if (la.kind == 110) {
			lexer.NextToken();

#line  1501 "cs.ATG" 
			List<SwitchSection> switchSections = new List<SwitchSection>(); 
			Expect(20);
			Expr(
#line  1502 "cs.ATG" 
out expr);
			Expect(21);
			Expect(16);
			SwitchSections(
#line  1503 "cs.ATG" 
switchSections);
			Expect(17);

#line  1505 "cs.ATG" 
			statement = new SwitchStatement(expr, switchSections); 

#line  1506 "cs.ATG" 
			SetParent(switchSections, statement); 
		} else if (la.kind == 125) {
			lexer.NextToken();
			Expect(20);
			Expr(
#line  1509 "cs.ATG" 
out expr);
			Expect(21);
			EmbeddedStatement(
#line  1510 "cs.ATG" 
out embeddedStatement);

#line  1511 "cs.ATG" 
			statement = new DoLoopStatement(expr, embeddedStatement, ConditionType.While, ConditionPosition.Start);
		} else if (la.kind == 65) {
			lexer.NextToken();
			EmbeddedStatement(
#line  1513 "cs.ATG" 
out embeddedStatement);
			Expect(125);
			Expect(20);
			Expr(
#line  1514 "cs.ATG" 
out expr);
			Expect(21);
			Expect(11);

#line  1515 "cs.ATG" 
			statement = new DoLoopStatement(expr, embeddedStatement, ConditionType.While, ConditionPosition.End); 
		} else if (la.kind == 76) {
			lexer.NextToken();

#line  1517 "cs.ATG" 
			List<Statement> initializer = null; List<Statement> iterator = null; 
			Expect(20);
			if (StartOf(6)) {
				ForInitializer(
#line  1518 "cs.ATG" 
out initializer);
			}
			Expect(11);
			if (StartOf(6)) {
				Expr(
#line  1519 "cs.ATG" 
out expr);
			}
			Expect(11);
			if (StartOf(6)) {
				ForIterator(
#line  1520 "cs.ATG" 
out iterator);
			}
			Expect(21);
			EmbeddedStatement(
#line  1521 "cs.ATG" 
out embeddedStatement);

#line  1522 "cs.ATG" 
			statement = new ForStatement(initializer, expr, iterator, embeddedStatement); 
			SetParent(initializer, statement);
			SetParent(iterator, statement);
			
		} else if (la.kind == 77) {
			lexer.NextToken();
			Expect(20);
			Type(
#line  1527 "cs.ATG" 
out type);
			Identifier();

#line  1527 "cs.ATG" 
			string varName = t.val; 
			Expect(81);
			Expr(
#line  1528 "cs.ATG" 
out expr);
			Expect(21);
			EmbeddedStatement(
#line  1529 "cs.ATG" 
out embeddedStatement);

#line  1530 "cs.ATG" 
			statement = new ForeachStatement(type, varName , expr, embeddedStatement); 
		} else if (la.kind == 53) {
			lexer.NextToken();
			Expect(11);

#line  1533 "cs.ATG" 
			statement = new BreakStatement(); 
		} else if (la.kind == 61) {
			lexer.NextToken();
			Expect(11);

#line  1534 "cs.ATG" 
			statement = new ContinueStatement(); 
		} else if (la.kind == 78) {
			GotoStatement(
#line  1535 "cs.ATG" 
out statement);
		} else if (
#line  1537 "cs.ATG" 
IsYieldStatement()) {
			Expect(132);
			if (la.kind == 101) {
				lexer.NextToken();
				Expr(
#line  1538 "cs.ATG" 
out expr);

#line  1538 "cs.ATG" 
				statement = new YieldStatement(new ReturnStatement(expr)); 
			} else if (la.kind == 53) {
				lexer.NextToken();

#line  1539 "cs.ATG" 
				statement = new YieldStatement(new BreakStatement()); 
			} else SynErr(198);
			Expect(11);
		} else if (la.kind == 101) {
			lexer.NextToken();
			if (StartOf(6)) {
				Expr(
#line  1542 "cs.ATG" 
out expr);
			}
			Expect(11);

#line  1542 "cs.ATG" 
			statement = new ReturnStatement(expr); 
		} else if (la.kind == 112) {
			lexer.NextToken();
			if (StartOf(6)) {
				Expr(
#line  1543 "cs.ATG" 
out expr);
			}
			Expect(11);

#line  1543 "cs.ATG" 
			statement = new ThrowStatement(expr); 
		} else if (StartOf(6)) {
			StatementExpr(
#line  1546 "cs.ATG" 
out statement);
			while (!(la.kind == 0 || la.kind == 11)) {SynErr(199); lexer.NextToken(); }
			Expect(11);
		} else if (la.kind == 114) {
			TryStatement(
#line  1549 "cs.ATG" 
out statement);
		} else if (la.kind == 86) {
			lexer.NextToken();
			Expect(20);
			Expr(
#line  1552 "cs.ATG" 
out expr);
			Expect(21);
			EmbeddedStatement(
#line  1553 "cs.ATG" 
out embeddedStatement);

#line  1553 "cs.ATG" 
			statement = new LockStatement(expr, embeddedStatement); 
		} else if (la.kind == 121) {

#line  1556 "cs.ATG" 
			Statement resourceAcquisitionStmt = null; 
			lexer.NextToken();
			Expect(20);
			ResourceAcquisition(
#line  1558 "cs.ATG" 
out resourceAcquisitionStmt);
			Expect(21);
			EmbeddedStatement(
#line  1559 "cs.ATG" 
out embeddedStatement);

#line  1559 "cs.ATG" 
			statement = new UsingStatement(resourceAcquisitionStmt, embeddedStatement); 
		} else if (la.kind == 119) {
			lexer.NextToken();
			Block(
#line  1562 "cs.ATG" 
out embeddedStatement);

#line  1562 "cs.ATG" 
			statement = new UnsafeStatement(embeddedStatement); 
		} else if (la.kind == 74) {

#line  1564 "cs.ATG" 
			Statement pointerDeclarationStmt = null; 
			lexer.NextToken();
			Expect(20);
			ResourceAcquisition(
#line  1566 "cs.ATG" 
out pointerDeclarationStmt);
			Expect(21);
			EmbeddedStatement(
#line  1567 "cs.ATG" 
out embeddedStatement);

#line  1567 "cs.ATG" 
			statement = new FixedStatement(pointerDeclarationStmt, embeddedStatement); 
		} else SynErr(200);

#line  1569 "cs.ATG" 
		if (statement != null) {
		statement.StartLocation = startLocation;
		statement.EndLocation = t.EndLocation;
		}
		
	}

	void IfStatement(
#line  1576 "cs.ATG" 
out Statement statement) {

#line  1578 "cs.ATG" 
		Expression expr = null;
		Statement embeddedStatement = null;
		statement = null;
		
		Expect(79);
		Expect(20);
		Expr(
#line  1584 "cs.ATG" 
out expr);
		Expect(21);
		EmbeddedStatement(
#line  1585 "cs.ATG" 
out embeddedStatement);

#line  1586 "cs.ATG" 
		Statement elseStatement = null; 
		if (la.kind == 67) {
			lexer.NextToken();
			EmbeddedStatement(
#line  1587 "cs.ATG" 
out elseStatement);
		}

#line  1588 "cs.ATG" 
		statement = elseStatement != null ? new IfElseStatement(expr, embeddedStatement, elseStatement) : new IfElseStatement(expr, embeddedStatement); 

#line  1589 "cs.ATG" 
		if (elseStatement is IfElseStatement && (elseStatement as IfElseStatement).TrueStatement.Count == 1) {
		/* else if-section (otherwise we would have a BlockStatment) */
		(statement as IfElseStatement).ElseIfSections.Add(
		             new ElseIfSection((elseStatement as IfElseStatement).Condition,
		                               (elseStatement as IfElseStatement).TrueStatement[0]));
		(statement as IfElseStatement).ElseIfSections.AddRange((elseStatement as IfElseStatement).ElseIfSections);
		(statement as IfElseStatement).FalseStatement = (elseStatement as IfElseStatement).FalseStatement;
		SetParent((statement as IfElseStatement).FalseStatement, statement);
		SetParent((statement as IfElseStatement).ElseIfSections, statement);
		}
		
	}

	void SwitchSections(
#line  1621 "cs.ATG" 
List<SwitchSection> switchSections) {

#line  1623 "cs.ATG" 
		SwitchSection switchSection = new SwitchSection();
		CaseLabel label;
		
		SwitchLabel(
#line  1627 "cs.ATG" 
out label);

#line  1627 "cs.ATG" 
		SafeAdd(switchSection, switchSection.SwitchLabels, label); 

#line  1628 "cs.ATG" 
		compilationUnit.BlockStart(switchSection); 
		while (StartOf(32)) {
			if (la.kind == 55 || la.kind == 63) {
				SwitchLabel(
#line  1630 "cs.ATG" 
out label);

#line  1631 "cs.ATG" 
				if (label != null) {
				if (switchSection.Children.Count > 0) {
					// open new section
					compilationUnit.BlockEnd(); switchSections.Add(switchSection);
					switchSection = new SwitchSection();
					compilationUnit.BlockStart(switchSection);
				}
				SafeAdd(switchSection, switchSection.SwitchLabels, label);
				}
				
			} else {
				Statement();
			}
		}

#line  1643 "cs.ATG" 
		compilationUnit.BlockEnd(); switchSections.Add(switchSection); 
	}

	void ForInitializer(
#line  1602 "cs.ATG" 
out List<Statement> initializer) {

#line  1604 "cs.ATG" 
		Statement stmt; 
		initializer = new List<Statement>();
		
		if (
#line  1608 "cs.ATG" 
IsLocalVarDecl()) {
			LocalVariableDecl(
#line  1608 "cs.ATG" 
out stmt);

#line  1608 "cs.ATG" 
			initializer.Add(stmt);
		} else if (StartOf(6)) {
			StatementExpr(
#line  1609 "cs.ATG" 
out stmt);

#line  1609 "cs.ATG" 
			initializer.Add(stmt);
			while (la.kind == 14) {
				lexer.NextToken();
				StatementExpr(
#line  1609 "cs.ATG" 
out stmt);

#line  1609 "cs.ATG" 
				initializer.Add(stmt);
			}
		} else SynErr(201);
	}

	void ForIterator(
#line  1612 "cs.ATG" 
out List<Statement> iterator) {

#line  1614 "cs.ATG" 
		Statement stmt; 
		iterator = new List<Statement>();
		
		StatementExpr(
#line  1618 "cs.ATG" 
out stmt);

#line  1618 "cs.ATG" 
		iterator.Add(stmt);
		while (la.kind == 14) {
			lexer.NextToken();
			StatementExpr(
#line  1618 "cs.ATG" 
out stmt);

#line  1618 "cs.ATG" 
			iterator.Add(stmt); 
		}
	}

	void GotoStatement(
#line  1698 "cs.ATG" 
out Statement stmt) {

#line  1699 "cs.ATG" 
		Expression expr; stmt = null; 
		Expect(78);
		if (StartOf(19)) {
			Identifier();

#line  1703 "cs.ATG" 
			stmt = new GotoStatement(t.val); 
			Expect(11);
		} else if (la.kind == 55) {
			lexer.NextToken();
			Expr(
#line  1704 "cs.ATG" 
out expr);
			Expect(11);

#line  1704 "cs.ATG" 
			stmt = new GotoCaseStatement(expr); 
		} else if (la.kind == 63) {
			lexer.NextToken();
			Expect(11);

#line  1705 "cs.ATG" 
			stmt = new GotoCaseStatement(null); 
		} else SynErr(202);
	}

	void StatementExpr(
#line  1725 "cs.ATG" 
out Statement stmt) {

#line  1726 "cs.ATG" 
		Expression expr; 
		Expr(
#line  1728 "cs.ATG" 
out expr);

#line  1731 "cs.ATG" 
		stmt = new ExpressionStatement(expr); 
	}

	void TryStatement(
#line  1653 "cs.ATG" 
out Statement tryStatement) {

#line  1655 "cs.ATG" 
		Statement blockStmt = null, finallyStmt = null;
		List<CatchClause> catchClauses = null;
		
		Expect(114);
		Block(
#line  1659 "cs.ATG" 
out blockStmt);
		if (la.kind == 56) {
			CatchClauses(
#line  1661 "cs.ATG" 
out catchClauses);
			if (la.kind == 73) {
				lexer.NextToken();
				Block(
#line  1661 "cs.ATG" 
out finallyStmt);
			}
		} else if (la.kind == 73) {
			lexer.NextToken();
			Block(
#line  1662 "cs.ATG" 
out finallyStmt);
		} else SynErr(203);

#line  1665 "cs.ATG" 
		tryStatement = new TryCatchStatement(blockStmt, catchClauses, finallyStmt);
		if (catchClauses != null) {
			foreach (CatchClause cc in catchClauses) cc.Parent = tryStatement;
		}
		
	}

	void ResourceAcquisition(
#line  1709 "cs.ATG" 
out Statement stmt) {

#line  1711 "cs.ATG" 
		stmt = null;
		Expression expr;
		
		if (
#line  1716 "cs.ATG" 
IsLocalVarDecl()) {
			LocalVariableDecl(
#line  1716 "cs.ATG" 
out stmt);
		} else if (StartOf(6)) {
			Expr(
#line  1717 "cs.ATG" 
out expr);

#line  1721 "cs.ATG" 
			stmt = new ExpressionStatement(expr); 
		} else SynErr(204);
	}

	void SwitchLabel(
#line  1646 "cs.ATG" 
out CaseLabel label) {

#line  1647 "cs.ATG" 
		Expression expr = null; label = null; 
		if (la.kind == 55) {
			lexer.NextToken();
			Expr(
#line  1649 "cs.ATG" 
out expr);
			Expect(9);

#line  1649 "cs.ATG" 
			label =  new CaseLabel(expr); 
		} else if (la.kind == 63) {
			lexer.NextToken();
			Expect(9);

#line  1650 "cs.ATG" 
			label =  new CaseLabel(); 
		} else SynErr(205);
	}

	void CatchClauses(
#line  1672 "cs.ATG" 
out List<CatchClause> catchClauses) {

#line  1674 "cs.ATG" 
		catchClauses = new List<CatchClause>();
		
		Expect(56);

#line  1677 "cs.ATG" 
		string identifier;
		Statement stmt;
		TypeReference typeRef;
		
		if (la.kind == 16) {
			Block(
#line  1683 "cs.ATG" 
out stmt);

#line  1683 "cs.ATG" 
			catchClauses.Add(new CatchClause(stmt)); 
		} else if (la.kind == 20) {
			lexer.NextToken();
			ClassType(
#line  1685 "cs.ATG" 
out typeRef, false);

#line  1685 "cs.ATG" 
			identifier = null; 
			if (StartOf(19)) {
				Identifier();

#line  1686 "cs.ATG" 
				identifier = t.val; 
			}
			Expect(21);
			Block(
#line  1687 "cs.ATG" 
out stmt);

#line  1688 "cs.ATG" 
			catchClauses.Add(new CatchClause(typeRef, identifier, stmt)); 
			while (
#line  1689 "cs.ATG" 
IsTypedCatch()) {
				Expect(56);
				Expect(20);
				ClassType(
#line  1689 "cs.ATG" 
out typeRef, false);

#line  1689 "cs.ATG" 
				identifier = null; 
				if (StartOf(19)) {
					Identifier();

#line  1690 "cs.ATG" 
					identifier = t.val; 
				}
				Expect(21);
				Block(
#line  1691 "cs.ATG" 
out stmt);

#line  1692 "cs.ATG" 
				catchClauses.Add(new CatchClause(typeRef, identifier, stmt)); 
			}
			if (la.kind == 56) {
				lexer.NextToken();
				Block(
#line  1694 "cs.ATG" 
out stmt);

#line  1694 "cs.ATG" 
				catchClauses.Add(new CatchClause(stmt)); 
			}
		} else SynErr(206);
	}

	void UnaryExpr(
#line  1758 "cs.ATG" 
out Expression uExpr) {

#line  1760 "cs.ATG" 
		TypeReference type = null;
		Expression expr = null;
		ArrayList expressions = new ArrayList();
		uExpr = null;
		
		while (StartOf(33) || 
#line  1782 "cs.ATG" 
IsTypeCast()) {
			if (la.kind == 4) {
				lexer.NextToken();

#line  1769 "cs.ATG" 
				expressions.Add(new UnaryOperatorExpression(UnaryOperatorType.Plus)); 
			} else if (la.kind == 5) {
				lexer.NextToken();

#line  1770 "cs.ATG" 
				expressions.Add(new UnaryOperatorExpression(UnaryOperatorType.Minus)); 
			} else if (la.kind == 24) {
				lexer.NextToken();

#line  1771 "cs.ATG" 
				expressions.Add(new UnaryOperatorExpression(UnaryOperatorType.Not)); 
			} else if (la.kind == 27) {
				lexer.NextToken();

#line  1772 "cs.ATG" 
				expressions.Add(new UnaryOperatorExpression(UnaryOperatorType.BitNot)); 
			} else if (la.kind == 6) {
				lexer.NextToken();

#line  1773 "cs.ATG" 
				expressions.Add(new UnaryOperatorExpression(UnaryOperatorType.Dereference)); 
			} else if (la.kind == 31) {
				lexer.NextToken();

#line  1774 "cs.ATG" 
				expressions.Add(new UnaryOperatorExpression(UnaryOperatorType.Increment)); 
			} else if (la.kind == 32) {
				lexer.NextToken();

#line  1775 "cs.ATG" 
				expressions.Add(new UnaryOperatorExpression(UnaryOperatorType.Decrement)); 
			} else if (la.kind == 28) {
				lexer.NextToken();

#line  1776 "cs.ATG" 
				expressions.Add(new UnaryOperatorExpression(UnaryOperatorType.AddressOf)); 
			} else {
				Expect(20);
				Type(
#line  1782 "cs.ATG" 
out type);
				Expect(21);

#line  1782 "cs.ATG" 
				expressions.Add(new CastExpression(type)); 
			}
		}
		if (
#line  1787 "cs.ATG" 
LastExpressionIsUnaryMinus(expressions) && IsMostNegativeIntegerWithoutTypeSuffix()) {
			Expect(2);

#line  1790 "cs.ATG" 
			expressions.RemoveAt(expressions.Count - 1);
			if (t.literalValue is uint) {
				expr = new PrimitiveExpression(int.MinValue, int.MinValue.ToString());
			} else if (t.literalValue is ulong) {
				expr = new PrimitiveExpression(long.MinValue, long.MinValue.ToString());
			} else {
				throw new Exception("t.literalValue must be uint or ulong");
			}
			
		} else if (StartOf(34)) {
			PrimaryExpr(
#line  1799 "cs.ATG" 
out expr);
		} else SynErr(207);

#line  1801 "cs.ATG" 
		for (int i = 0; i < expressions.Count; ++i) {
		Expression nextExpression = i + 1 < expressions.Count ? (Expression)expressions[i + 1] : expr;
		if (expressions[i] is CastExpression) {
			((CastExpression)expressions[i]).Expression = nextExpression;
		} else {
			((UnaryOperatorExpression)expressions[i]).Expression = nextExpression;
		}
		}
		if (expressions.Count > 0) {
			uExpr = (Expression)expressions[0];
		} else {
			uExpr = expr;
		}
		
	}

	void ConditionalOrExpr(
#line  2118 "cs.ATG" 
ref Expression outExpr) {

#line  2119 "cs.ATG" 
		Expression expr;   
		ConditionalAndExpr(
#line  2121 "cs.ATG" 
ref outExpr);
		while (la.kind == 26) {
			lexer.NextToken();
			UnaryExpr(
#line  2121 "cs.ATG" 
out expr);
			ConditionalAndExpr(
#line  2121 "cs.ATG" 
ref expr);

#line  2121 "cs.ATG" 
			outExpr = new BinaryOperatorExpression(outExpr, BinaryOperatorType.LogicalOr, expr);  
		}
	}

	void PrimaryExpr(
#line  1818 "cs.ATG" 
out Expression pexpr) {

#line  1820 "cs.ATG" 
		TypeReference type = null;
		Expression expr;
		pexpr = null;
		

#line  1825 "cs.ATG" 
		Location startLocation = la.Location; 
		if (la.kind == 113) {
			lexer.NextToken();

#line  1827 "cs.ATG" 
			pexpr = new PrimitiveExpression(true, "true");  
		} else if (la.kind == 72) {
			lexer.NextToken();

#line  1828 "cs.ATG" 
			pexpr = new PrimitiveExpression(false, "false"); 
		} else if (la.kind == 90) {
			lexer.NextToken();

#line  1829 "cs.ATG" 
			pexpr = new PrimitiveExpression(null, "null");  
		} else if (la.kind == 2) {
			lexer.NextToken();

#line  1830 "cs.ATG" 
			pexpr = new PrimitiveExpression(t.literalValue, t.val) { LiteralFormat = t.literalFormat };  
		} else if (
#line  1831 "cs.ATG" 
StartOfQueryExpression()) {
			QueryExpression(
#line  1832 "cs.ATG" 
out pexpr);
		} else if (
#line  1833 "cs.ATG" 
IdentAndDoubleColon()) {
			Identifier();

#line  1834 "cs.ATG" 
			type = new TypeReference(t.val); 
			Expect(10);

#line  1835 "cs.ATG" 
			pexpr = new TypeReferenceExpression(type); 
			Identifier();

#line  1836 "cs.ATG" 
			if (type.Type == "global") { type.IsGlobal = true; type.Type = (t.val ?? "?"); } else type.Type += "." + (t.val ?? "?"); 
		} else if (StartOf(19)) {
			Identifier();

#line  1840 "cs.ATG" 
			pexpr = new IdentifierExpression(t.val); 
			if (la.kind == 48 || 
#line  1843 "cs.ATG" 
IsGenericInSimpleNameOrMemberAccess()) {
				if (la.kind == 48) {
					ShortedLambdaExpression(
#line  1842 "cs.ATG" 
(IdentifierExpression)pexpr, out pexpr);
				} else {

#line  1844 "cs.ATG" 
					List<TypeReference> typeList; 
					TypeArgumentList(
#line  1845 "cs.ATG" 
out typeList, false);

#line  1846 "cs.ATG" 
					((IdentifierExpression)pexpr).TypeArguments = typeList; 
				}
			}
		} else if (
#line  1848 "cs.ATG" 
IsLambdaExpression()) {
			LambdaExpression(
#line  1849 "cs.ATG" 
out pexpr);
		} else if (la.kind == 20) {
			lexer.NextToken();
			Expr(
#line  1852 "cs.ATG" 
out expr);
			Expect(21);

#line  1852 "cs.ATG" 
			pexpr = new ParenthesizedExpression(expr); 
		} else if (StartOf(35)) {

#line  1855 "cs.ATG" 
			string val = null; 
			switch (la.kind) {
			case 52: {
				lexer.NextToken();

#line  1856 "cs.ATG" 
				val = "System.Boolean"; 
				break;
			}
			case 54: {
				lexer.NextToken();

#line  1857 "cs.ATG" 
				val = "System.Byte"; 
				break;
			}
			case 57: {
				lexer.NextToken();

#line  1858 "cs.ATG" 
				val = "System.Char"; 
				break;
			}
			case 62: {
				lexer.NextToken();

#line  1859 "cs.ATG" 
				val = "System.Decimal"; 
				break;
			}
			case 66: {
				lexer.NextToken();

#line  1860 "cs.ATG" 
				val = "System.Double"; 
				break;
			}
			case 75: {
				lexer.NextToken();

#line  1861 "cs.ATG" 
				val = "System.Single"; 
				break;
			}
			case 82: {
				lexer.NextToken();

#line  1862 "cs.ATG" 
				val = "System.Int32"; 
				break;
			}
			case 87: {
				lexer.NextToken();

#line  1863 "cs.ATG" 
				val = "System.Int64"; 
				break;
			}
			case 91: {
				lexer.NextToken();

#line  1864 "cs.ATG" 
				val = "System.Object"; 
				break;
			}
			case 102: {
				lexer.NextToken();

#line  1865 "cs.ATG" 
				val = "System.SByte"; 
				break;
			}
			case 104: {
				lexer.NextToken();

#line  1866 "cs.ATG" 
				val = "System.Int16"; 
				break;
			}
			case 108: {
				lexer.NextToken();

#line  1867 "cs.ATG" 
				val = "System.String"; 
				break;
			}
			case 116: {
				lexer.NextToken();

#line  1868 "cs.ATG" 
				val = "System.UInt32"; 
				break;
			}
			case 117: {
				lexer.NextToken();

#line  1869 "cs.ATG" 
				val = "System.UInt64"; 
				break;
			}
			case 120: {
				lexer.NextToken();

#line  1870 "cs.ATG" 
				val = "System.UInt16"; 
				break;
			}
			case 123: {
				lexer.NextToken();

#line  1871 "cs.ATG" 
				val = "System.Void"; 
				break;
			}
			}
			MemberAccess(
#line  1873 "cs.ATG" 
out pexpr, new TypeReferenceExpression(new TypeReference(val, true)) { StartLocation = t.Location, EndLocation = t.EndLocation } );
		} else if (la.kind == 111) {
			lexer.NextToken();

#line  1876 "cs.ATG" 
			pexpr = new ThisReferenceExpression(); 
		} else if (la.kind == 51) {
			lexer.NextToken();

#line  1878 "cs.ATG" 
			pexpr = new BaseReferenceExpression(); 
		} else if (la.kind == 89) {
			NewExpression(
#line  1881 "cs.ATG" 
out pexpr);
		} else if (la.kind == 115) {
			lexer.NextToken();
			Expect(20);
			if (
#line  1885 "cs.ATG" 
NotVoidPointer()) {
				Expect(123);

#line  1885 "cs.ATG" 
				type = new TypeReference("System.Void", true); 
			} else if (StartOf(10)) {
				TypeWithRestriction(
#line  1886 "cs.ATG" 
out type, true, true);
			} else SynErr(208);
			Expect(21);

#line  1888 "cs.ATG" 
			pexpr = new TypeOfExpression(type); 
		} else if (la.kind == 63) {
			lexer.NextToken();
			Expect(20);
			Type(
#line  1890 "cs.ATG" 
out type);
			Expect(21);

#line  1890 "cs.ATG" 
			pexpr = new DefaultValueExpression(type); 
		} else if (la.kind == 105) {
			lexer.NextToken();
			Expect(20);
			Type(
#line  1891 "cs.ATG" 
out type);
			Expect(21);

#line  1891 "cs.ATG" 
			pexpr = new SizeOfExpression(type); 
		} else if (la.kind == 58) {
			lexer.NextToken();
			Expect(20);
			Expr(
#line  1892 "cs.ATG" 
out expr);
			Expect(21);

#line  1892 "cs.ATG" 
			pexpr = new CheckedExpression(expr); 
		} else if (la.kind == 118) {
			lexer.NextToken();
			Expect(20);
			Expr(
#line  1893 "cs.ATG" 
out expr);
			Expect(21);

#line  1893 "cs.ATG" 
			pexpr = new UncheckedExpression(expr); 
		} else if (la.kind == 64) {
			lexer.NextToken();
			AnonymousMethodExpr(
#line  1894 "cs.ATG" 
out expr);

#line  1894 "cs.ATG" 
			pexpr = expr; 
		} else SynErr(209);

#line  1896 "cs.ATG" 
		if (pexpr != null) {
		pexpr.StartLocation = startLocation;
		pexpr.EndLocation = t.EndLocation;
		}
		
		while (StartOf(36)) {
			if (la.kind == 31 || la.kind == 32) {

#line  1902 "cs.ATG" 
				startLocation = la.Location; 
				if (la.kind == 31) {
					lexer.NextToken();

#line  1904 "cs.ATG" 
					pexpr = new UnaryOperatorExpression(pexpr, UnaryOperatorType.PostIncrement); 
				} else if (la.kind == 32) {
					lexer.NextToken();

#line  1905 "cs.ATG" 
					pexpr = new UnaryOperatorExpression(pexpr, UnaryOperatorType.PostDecrement); 
				} else SynErr(210);
			} else if (la.kind == 47) {
				PointerMemberAccess(
#line  1908 "cs.ATG" 
out pexpr, pexpr);
			} else if (la.kind == 15) {
				MemberAccess(
#line  1909 "cs.ATG" 
out pexpr, pexpr);
			} else if (la.kind == 20) {
				lexer.NextToken();

#line  1913 "cs.ATG" 
				List<Expression> parameters = new List<Expression>(); 

#line  1914 "cs.ATG" 
				pexpr = new InvocationExpression(pexpr, parameters); 
				if (StartOf(26)) {
					Argument(
#line  1915 "cs.ATG" 
out expr);

#line  1915 "cs.ATG" 
					SafeAdd(pexpr, parameters, expr); 
					while (la.kind == 14) {
						lexer.NextToken();
						Argument(
#line  1916 "cs.ATG" 
out expr);

#line  1916 "cs.ATG" 
						SafeAdd(pexpr, parameters, expr); 
					}
				}
				Expect(21);
			} else {

#line  1922 "cs.ATG" 
				List<Expression> indices = new List<Expression>();
				pexpr = new IndexerExpression(pexpr, indices);
				
				lexer.NextToken();
				Expr(
#line  1925 "cs.ATG" 
out expr);

#line  1925 "cs.ATG" 
				SafeAdd(pexpr, indices, expr); 
				while (la.kind == 14) {
					lexer.NextToken();
					Expr(
#line  1926 "cs.ATG" 
out expr);

#line  1926 "cs.ATG" 
					SafeAdd(pexpr, indices, expr); 
				}
				Expect(19);

#line  1929 "cs.ATG" 
				if (pexpr != null) {
				pexpr.StartLocation = startLocation;
				pexpr.EndLocation = t.EndLocation;
				}
				
			}
		}
	}

	void QueryExpression(
#line  2354 "cs.ATG" 
out Expression outExpr) {

#line  2355 "cs.ATG" 
		QueryExpression q = new QueryExpression(); outExpr = q; q.StartLocation = la.Location; 
		QueryExpressionFromClause fromClause;
		
		QueryExpressionFromClause(
#line  2359 "cs.ATG" 
out fromClause);

#line  2359 "cs.ATG" 
		q.FromClause = fromClause; 
		QueryExpressionBody(
#line  2360 "cs.ATG" 
ref q);

#line  2361 "cs.ATG" 
		q.EndLocation = t.EndLocation; 
		outExpr = q; /* set outExpr to q again if QueryExpressionBody changed it (can happen with 'into' clauses) */ 
		
	}

	void ShortedLambdaExpression(
#line  2042 "cs.ATG" 
IdentifierExpression ident, out Expression pexpr) {

#line  2043 "cs.ATG" 
		LambdaExpression lambda = new LambdaExpression(); pexpr = lambda; 
		Expect(48);

#line  2048 "cs.ATG" 
		lambda.StartLocation = ident.StartLocation;
		lambda.Parameters.Add(new ParameterDeclarationExpression(null, ident.Identifier));
		lambda.Parameters[0].StartLocation = ident.StartLocation;
		lambda.Parameters[0].EndLocation = ident.EndLocation;
		
		LambdaExpressionBody(
#line  2053 "cs.ATG" 
lambda);
	}

	void TypeArgumentList(
#line  2288 "cs.ATG" 
out List<TypeReference> types, bool canBeUnbound) {

#line  2290 "cs.ATG" 
		types = new List<TypeReference>();
		TypeReference type = null;
		
		Expect(23);
		if (
#line  2295 "cs.ATG" 
canBeUnbound && (la.kind == Tokens.GreaterThan || la.kind == Tokens.Comma)) {

#line  2296 "cs.ATG" 
			types.Add(TypeReference.Null); 
			while (la.kind == 14) {
				lexer.NextToken();

#line  2297 "cs.ATG" 
				types.Add(TypeReference.Null); 
			}
		} else if (StartOf(10)) {
			Type(
#line  2298 "cs.ATG" 
out type);

#line  2298 "cs.ATG" 
			if (type != null) { types.Add(type); } 
			while (la.kind == 14) {
				lexer.NextToken();
				Type(
#line  2299 "cs.ATG" 
out type);

#line  2299 "cs.ATG" 
				if (type != null) { types.Add(type); } 
			}
		} else SynErr(211);
		Expect(22);
	}

	void LambdaExpression(
#line  2022 "cs.ATG" 
out Expression outExpr) {

#line  2024 "cs.ATG" 
		LambdaExpression lambda = new LambdaExpression();
		lambda.StartLocation = la.Location;
		ParameterDeclarationExpression p;
		outExpr = lambda;
		
		Expect(20);
		if (StartOf(10)) {
			LambdaExpressionParameter(
#line  2032 "cs.ATG" 
out p);

#line  2032 "cs.ATG" 
			SafeAdd(lambda, lambda.Parameters, p); 
			while (la.kind == 14) {
				lexer.NextToken();
				LambdaExpressionParameter(
#line  2034 "cs.ATG" 
out p);

#line  2034 "cs.ATG" 
				SafeAdd(lambda, lambda.Parameters, p); 
			}
		}
		Expect(21);
		Expect(48);
		LambdaExpressionBody(
#line  2039 "cs.ATG" 
lambda);
	}

	void MemberAccess(
#line  1937 "cs.ATG" 
out Expression expr, Expression target) {

#line  1938 "cs.ATG" 
		List<TypeReference> typeList; 

#line  1940 "cs.ATG" 
		if (ShouldConvertTargetExpressionToTypeReference(target)) {
		TypeReference type = GetTypeReferenceFromExpression(target);
		if (type != null) {
			target = new TypeReferenceExpression(type) { StartLocation = t.Location, EndLocation = t.EndLocation };
		}
		}
		t.val = ""; // required for TypeReferenceExpressionTests.StandaloneIntReferenceExpression hack
		
		Expect(15);
		Identifier();

#line  1950 "cs.ATG" 
		expr = new MemberReferenceExpression(target, t.val); 
		if (
#line  1951 "cs.ATG" 
IsGenericInSimpleNameOrMemberAccess()) {
			TypeArgumentList(
#line  1952 "cs.ATG" 
out typeList, false);

#line  1953 "cs.ATG" 
			((MemberReferenceExpression)expr).TypeArguments = typeList; 
		}
	}

	void NewExpression(
#line  1969 "cs.ATG" 
out Expression pexpr) {

#line  1970 "cs.ATG" 
		pexpr = null;
		List<Expression> parameters = new List<Expression>();
		TypeReference type = null;
		Expression expr;
		
		Expect(89);
		if (StartOf(10)) {
			NonArrayType(
#line  1977 "cs.ATG" 
out type);
		}
		if (la.kind == 16 || la.kind == 20) {
			if (la.kind == 20) {

#line  1983 "cs.ATG" 
				ObjectCreateExpression oce = new ObjectCreateExpression(type, parameters); 
				lexer.NextToken();

#line  1984 "cs.ATG" 
				if (type == null) Error("Cannot use an anonymous type with arguments for the constructor"); 
				if (StartOf(26)) {
					Argument(
#line  1985 "cs.ATG" 
out expr);

#line  1985 "cs.ATG" 
					SafeAdd(oce, parameters, expr); 
					while (la.kind == 14) {
						lexer.NextToken();
						Argument(
#line  1986 "cs.ATG" 
out expr);

#line  1986 "cs.ATG" 
						SafeAdd(oce, parameters, expr); 
					}
				}
				Expect(21);

#line  1988 "cs.ATG" 
				pexpr = oce; 
				if (la.kind == 16) {
					CollectionOrObjectInitializer(
#line  1989 "cs.ATG" 
out expr);

#line  1989 "cs.ATG" 
					oce.ObjectInitializer = (CollectionInitializerExpression)expr; 
				}
			} else {

#line  1990 "cs.ATG" 
				ObjectCreateExpression oce = new ObjectCreateExpression(type, parameters); 
				CollectionOrObjectInitializer(
#line  1991 "cs.ATG" 
out expr);

#line  1991 "cs.ATG" 
				oce.ObjectInitializer = (CollectionInitializerExpression)expr; 

#line  1992 "cs.ATG" 
				pexpr = oce; 
			}
		} else if (la.kind == 18) {
			lexer.NextToken();

#line  1997 "cs.ATG" 
			ArrayCreateExpression ace = new ArrayCreateExpression(type);
			/* we must not change RankSpecifier on the null type reference*/
			if (ace.CreateType.IsNull) { ace.CreateType = new TypeReference(""); }
			pexpr = ace;
			int dims = 0; List<int> ranks = new List<int>();
			
			if (la.kind == 14 || la.kind == 19) {
				while (la.kind == 14) {
					lexer.NextToken();

#line  2004 "cs.ATG" 
					dims += 1; 
				}
				Expect(19);

#line  2005 "cs.ATG" 
				ranks.Add(dims); dims = 0; 
				while (la.kind == 18) {
					lexer.NextToken();
					while (la.kind == 14) {
						lexer.NextToken();

#line  2006 "cs.ATG" 
						++dims; 
					}
					Expect(19);

#line  2006 "cs.ATG" 
					ranks.Add(dims); dims = 0; 
				}

#line  2007 "cs.ATG" 
				ace.CreateType.RankSpecifier = ranks.ToArray(); 
				CollectionInitializer(
#line  2008 "cs.ATG" 
out expr);

#line  2008 "cs.ATG" 
				ace.ArrayInitializer = (CollectionInitializerExpression)expr; 
			} else if (StartOf(6)) {
				Expr(
#line  2009 "cs.ATG" 
out expr);

#line  2009 "cs.ATG" 
				if (expr != null) parameters.Add(expr); 
				while (la.kind == 14) {
					lexer.NextToken();

#line  2010 "cs.ATG" 
					dims += 1; 
					Expr(
#line  2011 "cs.ATG" 
out expr);

#line  2011 "cs.ATG" 
					if (expr != null) parameters.Add(expr); 
				}
				Expect(19);

#line  2013 "cs.ATG" 
				ranks.Add(dims); ace.Arguments = parameters; dims = 0; 
				while (la.kind == 18) {
					lexer.NextToken();
					while (la.kind == 14) {
						lexer.NextToken();

#line  2014 "cs.ATG" 
						++dims; 
					}
					Expect(19);

#line  2014 "cs.ATG" 
					ranks.Add(dims); dims = 0; 
				}

#line  2015 "cs.ATG" 
				ace.CreateType.RankSpecifier = ranks.ToArray(); 
				if (la.kind == 16) {
					CollectionInitializer(
#line  2016 "cs.ATG" 
out expr);

#line  2016 "cs.ATG" 
					ace.ArrayInitializer = (CollectionInitializerExpression)expr; 
				}
			} else SynErr(212);
		} else SynErr(213);
	}

	void AnonymousMethodExpr(
#line  2085 "cs.ATG" 
out Expression outExpr) {

#line  2087 "cs.ATG" 
		AnonymousMethodExpression expr = new AnonymousMethodExpression();
		expr.StartLocation = t.Location;
		BlockStatement stmt;
		List<ParameterDeclarationExpression> p = new List<ParameterDeclarationExpression>();
		outExpr = expr;
		
		if (la.kind == 20) {
			lexer.NextToken();
			if (StartOf(11)) {
				FormalParameterList(
#line  2096 "cs.ATG" 
p);

#line  2096 "cs.ATG" 
				expr.Parameters = p; 
			}
			Expect(21);

#line  2098 "cs.ATG" 
			expr.HasParameterList = true; 
		}
		BlockInsideExpression(
#line  2100 "cs.ATG" 
out stmt);

#line  2100 "cs.ATG" 
		expr.Body  = stmt; 

#line  2101 "cs.ATG" 
		expr.EndLocation = t.Location; 
	}

	void PointerMemberAccess(
#line  1957 "cs.ATG" 
out Expression expr, Expression target) {

#line  1958 "cs.ATG" 
		List<TypeReference> typeList; 
		Expect(47);
		Identifier();

#line  1962 "cs.ATG" 
		expr = new PointerReferenceExpression(target, t.val); 
		if (
#line  1963 "cs.ATG" 
IsGenericInSimpleNameOrMemberAccess()) {
			TypeArgumentList(
#line  1964 "cs.ATG" 
out typeList, false);

#line  1965 "cs.ATG" 
			((MemberReferenceExpression)expr).TypeArguments = typeList; 
		}
	}

	void LambdaExpressionParameter(
#line  2056 "cs.ATG" 
out ParameterDeclarationExpression p) {

#line  2057 "cs.ATG" 
		Location start = la.Location; p = null;
		TypeReference type;
		
		if (
#line  2061 "cs.ATG" 
Peek(1).kind == Tokens.Comma || Peek(1).kind == Tokens.CloseParenthesis) {
			Identifier();

#line  2063 "cs.ATG" 
			p = new ParameterDeclarationExpression(null, t.val);
			p.StartLocation = start; p.EndLocation = t.EndLocation;
			
		} else if (StartOf(10)) {
			Type(
#line  2066 "cs.ATG" 
out type);
			Identifier();

#line  2068 "cs.ATG" 
			p = new ParameterDeclarationExpression(type, t.val);
			p.StartLocation = start; p.EndLocation = t.EndLocation;
			
		} else SynErr(214);
	}

	void LambdaExpressionBody(
#line  2074 "cs.ATG" 
LambdaExpression lambda) {

#line  2075 "cs.ATG" 
		Expression expr; BlockStatement stmt; 
		if (la.kind == 16) {
			BlockInsideExpression(
#line  2078 "cs.ATG" 
out stmt);

#line  2078 "cs.ATG" 
			lambda.StatementBody = stmt; 
		} else if (StartOf(6)) {
			Expr(
#line  2079 "cs.ATG" 
out expr);

#line  2079 "cs.ATG" 
			lambda.ExpressionBody = expr; 
		} else SynErr(215);

#line  2081 "cs.ATG" 
		lambda.EndLocation = t.EndLocation; 

#line  2082 "cs.ATG" 
		lambda.ExtendedEndLocation = la.Location; 
	}

	void BlockInsideExpression(
#line  2104 "cs.ATG" 
out BlockStatement outStmt) {

#line  2105 "cs.ATG" 
		Statement stmt = null; outStmt = null; 

#line  2109 "cs.ATG" 
		if (compilationUnit != null) { 
		Block(
#line  2110 "cs.ATG" 
out stmt);

#line  2110 "cs.ATG" 
		outStmt = (BlockStatement)stmt; 

#line  2111 "cs.ATG" 
		} else { 
		Expect(16);

#line  2113 "cs.ATG" 
		lexer.SkipCurrentBlock(0); 
		Expect(17);

#line  2115 "cs.ATG" 
		} 
	}

	void ConditionalAndExpr(
#line  2124 "cs.ATG" 
ref Expression outExpr) {

#line  2125 "cs.ATG" 
		Expression expr; 
		InclusiveOrExpr(
#line  2127 "cs.ATG" 
ref outExpr);
		while (la.kind == 25) {
			lexer.NextToken();
			UnaryExpr(
#line  2127 "cs.ATG" 
out expr);
			InclusiveOrExpr(
#line  2127 "cs.ATG" 
ref expr);

#line  2127 "cs.ATG" 
			outExpr = new BinaryOperatorExpression(outExpr, BinaryOperatorType.LogicalAnd, expr);  
		}
	}

	void InclusiveOrExpr(
#line  2130 "cs.ATG" 
ref Expression outExpr) {

#line  2131 "cs.ATG" 
		Expression expr; 
		ExclusiveOrExpr(
#line  2133 "cs.ATG" 
ref outExpr);
		while (la.kind == 29) {
			lexer.NextToken();
			UnaryExpr(
#line  2133 "cs.ATG" 
out expr);
			ExclusiveOrExpr(
#line  2133 "cs.ATG" 
ref expr);

#line  2133 "cs.ATG" 
			outExpr = new BinaryOperatorExpression(outExpr, BinaryOperatorType.BitwiseOr, expr);  
		}
	}

	void ExclusiveOrExpr(
#line  2136 "cs.ATG" 
ref Expression outExpr) {

#line  2137 "cs.ATG" 
		Expression expr; 
		AndExpr(
#line  2139 "cs.ATG" 
ref outExpr);
		while (la.kind == 30) {
			lexer.NextToken();
			UnaryExpr(
#line  2139 "cs.ATG" 
out expr);
			AndExpr(
#line  2139 "cs.ATG" 
ref expr);

#line  2139 "cs.ATG" 
			outExpr = new BinaryOperatorExpression(outExpr, BinaryOperatorType.ExclusiveOr, expr);  
		}
	}

	void AndExpr(
#line  2142 "cs.ATG" 
ref Expression outExpr) {

#line  2143 "cs.ATG" 
		Expression expr; 
		EqualityExpr(
#line  2145 "cs.ATG" 
ref outExpr);
		while (la.kind == 28) {
			lexer.NextToken();
			UnaryExpr(
#line  2145 "cs.ATG" 
out expr);
			EqualityExpr(
#line  2145 "cs.ATG" 
ref expr);

#line  2145 "cs.ATG" 
			outExpr = new BinaryOperatorExpression(outExpr, BinaryOperatorType.BitwiseAnd, expr);  
		}
	}

	void EqualityExpr(
#line  2148 "cs.ATG" 
ref Expression outExpr) {

#line  2150 "cs.ATG" 
		Expression expr;
		BinaryOperatorType op = BinaryOperatorType.None;
		
		RelationalExpr(
#line  2154 "cs.ATG" 
ref outExpr);
		while (la.kind == 33 || la.kind == 34) {
			if (la.kind == 34) {
				lexer.NextToken();

#line  2157 "cs.ATG" 
				op = BinaryOperatorType.InEquality; 
			} else {
				lexer.NextToken();

#line  2158 "cs.ATG" 
				op = BinaryOperatorType.Equality; 
			}
			UnaryExpr(
#line  2160 "cs.ATG" 
out expr);
			RelationalExpr(
#line  2160 "cs.ATG" 
ref expr);

#line  2160 "cs.ATG" 
			outExpr = new BinaryOperatorExpression(outExpr, op, expr);  
		}
	}

	void RelationalExpr(
#line  2164 "cs.ATG" 
ref Expression outExpr) {

#line  2166 "cs.ATG" 
		TypeReference type;
		Expression expr;
		BinaryOperatorType op = BinaryOperatorType.None;
		
		ShiftExpr(
#line  2171 "cs.ATG" 
ref outExpr);
		while (StartOf(37)) {
			if (StartOf(38)) {
				if (la.kind == 23) {
					lexer.NextToken();

#line  2173 "cs.ATG" 
					op = BinaryOperatorType.LessThan; 
				} else if (la.kind == 22) {
					lexer.NextToken();

#line  2174 "cs.ATG" 
					op = BinaryOperatorType.GreaterThan; 
				} else if (la.kind == 36) {
					lexer.NextToken();

#line  2175 "cs.ATG" 
					op = BinaryOperatorType.LessThanOrEqual; 
				} else if (la.kind == 35) {
					lexer.NextToken();

#line  2176 "cs.ATG" 
					op = BinaryOperatorType.GreaterThanOrEqual; 
				} else SynErr(216);
				UnaryExpr(
#line  2178 "cs.ATG" 
out expr);
				ShiftExpr(
#line  2179 "cs.ATG" 
ref expr);

#line  2180 "cs.ATG" 
				outExpr = new BinaryOperatorExpression(outExpr, op, expr); 
			} else {
				if (la.kind == 85) {
					lexer.NextToken();
					TypeWithRestriction(
#line  2183 "cs.ATG" 
out type, false, false);
					if (
#line  2184 "cs.ATG" 
la.kind == Tokens.Question && !IsPossibleExpressionStart(Peek(1).kind)) {
						NullableQuestionMark(
#line  2185 "cs.ATG" 
ref type);
					}

#line  2186 "cs.ATG" 
					outExpr = new TypeOfIsExpression(outExpr, type); 
				} else if (la.kind == 50) {
					lexer.NextToken();
					TypeWithRestriction(
#line  2188 "cs.ATG" 
out type, false, false);
					if (
#line  2189 "cs.ATG" 
la.kind == Tokens.Question && !IsPossibleExpressionStart(Peek(1).kind)) {
						NullableQuestionMark(
#line  2190 "cs.ATG" 
ref type);
					}

#line  2191 "cs.ATG" 
					outExpr = new CastExpression(type, outExpr, CastType.TryCast); 
				} else SynErr(217);
			}
		}
	}

	void ShiftExpr(
#line  2196 "cs.ATG" 
ref Expression outExpr) {

#line  2198 "cs.ATG" 
		Expression expr;
		BinaryOperatorType op = BinaryOperatorType.None;
		
		AdditiveExpr(
#line  2202 "cs.ATG" 
ref outExpr);
		while (la.kind == 37 || 
#line  2205 "cs.ATG" 
IsShiftRight()) {
			if (la.kind == 37) {
				lexer.NextToken();

#line  2204 "cs.ATG" 
				op = BinaryOperatorType.ShiftLeft; 
			} else {
				Expect(22);
				Expect(22);

#line  2206 "cs.ATG" 
				op = BinaryOperatorType.ShiftRight; 
			}
			UnaryExpr(
#line  2209 "cs.ATG" 
out expr);
			AdditiveExpr(
#line  2209 "cs.ATG" 
ref expr);

#line  2209 "cs.ATG" 
			outExpr = new BinaryOperatorExpression(outExpr, op, expr);  
		}
	}

	void AdditiveExpr(
#line  2213 "cs.ATG" 
ref Expression outExpr) {

#line  2215 "cs.ATG" 
		Expression expr;
		BinaryOperatorType op = BinaryOperatorType.None;
		
		MultiplicativeExpr(
#line  2219 "cs.ATG" 
ref outExpr);
		while (la.kind == 4 || la.kind == 5) {
			if (la.kind == 4) {
				lexer.NextToken();

#line  2222 "cs.ATG" 
				op = BinaryOperatorType.Add; 
			} else {
				lexer.NextToken();

#line  2223 "cs.ATG" 
				op = BinaryOperatorType.Subtract; 
			}
			UnaryExpr(
#line  2225 "cs.ATG" 
out expr);
			MultiplicativeExpr(
#line  2225 "cs.ATG" 
ref expr);

#line  2225 "cs.ATG" 
			outExpr = new BinaryOperatorExpression(outExpr, op, expr);  
		}
	}

	void MultiplicativeExpr(
#line  2229 "cs.ATG" 
ref Expression outExpr) {

#line  2231 "cs.ATG" 
		Expression expr;
		BinaryOperatorType op = BinaryOperatorType.None;
		
		while (la.kind == 6 || la.kind == 7 || la.kind == 8) {
			if (la.kind == 6) {
				lexer.NextToken();

#line  2237 "cs.ATG" 
				op = BinaryOperatorType.Multiply; 
			} else if (la.kind == 7) {
				lexer.NextToken();

#line  2238 "cs.ATG" 
				op = BinaryOperatorType.Divide; 
			} else {
				lexer.NextToken();

#line  2239 "cs.ATG" 
				op = BinaryOperatorType.Modulus; 
			}
			UnaryExpr(
#line  2241 "cs.ATG" 
out expr);

#line  2241 "cs.ATG" 
			outExpr = new BinaryOperatorExpression(outExpr, op, expr); 
		}
	}

	void TypeParameterConstraintsClauseBase(
#line  2345 "cs.ATG" 
out TypeReference type) {

#line  2346 "cs.ATG" 
		TypeReference t; type = null; 
		if (la.kind == 109) {
			lexer.NextToken();

#line  2348 "cs.ATG" 
			type = TypeReference.StructConstraint; 
		} else if (la.kind == 59) {
			lexer.NextToken();

#line  2349 "cs.ATG" 
			type = TypeReference.ClassConstraint; 
		} else if (la.kind == 89) {
			lexer.NextToken();
			Expect(20);
			Expect(21);

#line  2350 "cs.ATG" 
			type = TypeReference.NewConstraint; 
		} else if (StartOf(10)) {
			Type(
#line  2351 "cs.ATG" 
out t);

#line  2351 "cs.ATG" 
			type = t; 
		} else SynErr(218);
	}

	void QueryExpressionFromClause(
#line  2366 "cs.ATG" 
out QueryExpressionFromClause fc) {

#line  2367 "cs.ATG" 
		fc = new QueryExpressionFromClause(); fc.StartLocation = la.Location; 
		
		Expect(137);
		QueryExpressionFromOrJoinClause(
#line  2371 "cs.ATG" 
fc);

#line  2372 "cs.ATG" 
		fc.EndLocation = t.EndLocation; 
	}

	void QueryExpressionBody(
#line  2402 "cs.ATG" 
ref QueryExpression q) {

#line  2403 "cs.ATG" 
		QueryExpressionFromClause fromClause;     QueryExpressionWhereClause whereClause;
		QueryExpressionLetClause letClause;       QueryExpressionJoinClause joinClause;
		QueryExpressionOrderClause orderClause;
		QueryExpressionSelectClause selectClause; QueryExpressionGroupClause groupClause;
		
		while (StartOf(39)) {
			if (la.kind == 137) {
				QueryExpressionFromClause(
#line  2409 "cs.ATG" 
out fromClause);

#line  2409 "cs.ATG" 
				SafeAdd<QueryExpressionClause>(q, q.MiddleClauses, fromClause); 
			} else if (la.kind == 127) {
				QueryExpressionWhereClause(
#line  2410 "cs.ATG" 
out whereClause);

#line  2410 "cs.ATG" 
				SafeAdd<QueryExpressionClause>(q, q.MiddleClauses, whereClause); 
			} else if (la.kind == 141) {
				QueryExpressionLetClause(
#line  2411 "cs.ATG" 
out letClause);

#line  2411 "cs.ATG" 
				SafeAdd<QueryExpressionClause>(q, q.MiddleClauses, letClause); 
			} else if (la.kind == 142) {
				QueryExpressionJoinClause(
#line  2412 "cs.ATG" 
out joinClause);

#line  2412 "cs.ATG" 
				SafeAdd<QueryExpressionClause>(q, q.MiddleClauses, joinClause); 
			} else {
				QueryExpressionOrderByClause(
#line  2413 "cs.ATG" 
out orderClause);

#line  2413 "cs.ATG" 
				SafeAdd<QueryExpressionClause>(q, q.MiddleClauses, orderClause); 
			}
		}
		if (la.kind == 133) {
			QueryExpressionSelectClause(
#line  2415 "cs.ATG" 
out selectClause);

#line  2415 "cs.ATG" 
			q.SelectOrGroupClause = selectClause; 
		} else if (la.kind == 134) {
			QueryExpressionGroupClause(
#line  2416 "cs.ATG" 
out groupClause);

#line  2416 "cs.ATG" 
			q.SelectOrGroupClause = groupClause; 
		} else SynErr(219);
		if (la.kind == 136) {
			QueryExpressionIntoClause(
#line  2418 "cs.ATG" 
ref q);
		}
	}

	void QueryExpressionFromOrJoinClause(
#line  2392 "cs.ATG" 
QueryExpressionFromOrJoinClause fjc) {

#line  2393 "cs.ATG" 
		TypeReference type; Expression expr; 

#line  2395 "cs.ATG" 
		fjc.Type = null; 
		if (
#line  2396 "cs.ATG" 
IsLocalVarDecl()) {
			Type(
#line  2396 "cs.ATG" 
out type);

#line  2396 "cs.ATG" 
			fjc.Type = type; 
		}
		Identifier();

#line  2397 "cs.ATG" 
		fjc.Identifier = t.val; 
		Expect(81);
		Expr(
#line  2399 "cs.ATG" 
out expr);

#line  2399 "cs.ATG" 
		fjc.InExpression = expr; 
	}

	void QueryExpressionJoinClause(
#line  2375 "cs.ATG" 
out QueryExpressionJoinClause jc) {

#line  2376 "cs.ATG" 
		jc = new QueryExpressionJoinClause(); jc.StartLocation = la.Location; 
		Expression expr;
		
		Expect(142);
		QueryExpressionFromOrJoinClause(
#line  2381 "cs.ATG" 
jc);
		Expect(143);
		Expr(
#line  2383 "cs.ATG" 
out expr);

#line  2383 "cs.ATG" 
		jc.OnExpression = expr; 
		Expect(144);
		Expr(
#line  2385 "cs.ATG" 
out expr);

#line  2385 "cs.ATG" 
		jc.EqualsExpression = expr; 
		if (la.kind == 136) {
			lexer.NextToken();
			Identifier();

#line  2387 "cs.ATG" 
			jc.IntoIdentifier = t.val; 
		}

#line  2389 "cs.ATG" 
		jc.EndLocation = t.EndLocation; 
	}

	void QueryExpressionWhereClause(
#line  2421 "cs.ATG" 
out QueryExpressionWhereClause wc) {

#line  2422 "cs.ATG" 
		Expression expr; wc = new QueryExpressionWhereClause(); wc.StartLocation = la.Location; 
		Expect(127);
		Expr(
#line  2425 "cs.ATG" 
out expr);

#line  2425 "cs.ATG" 
		wc.Condition = expr; 

#line  2426 "cs.ATG" 
		wc.EndLocation = t.EndLocation; 
	}

	void QueryExpressionLetClause(
#line  2429 "cs.ATG" 
out QueryExpressionLetClause wc) {

#line  2430 "cs.ATG" 
		Expression expr; wc = new QueryExpressionLetClause(); wc.StartLocation = la.Location; 
		Expect(141);
		Identifier();

#line  2433 "cs.ATG" 
		wc.Identifier = t.val; 
		Expect(3);
		Expr(
#line  2435 "cs.ATG" 
out expr);

#line  2435 "cs.ATG" 
		wc.Expression = expr; 

#line  2436 "cs.ATG" 
		wc.EndLocation = t.EndLocation; 
	}

	void QueryExpressionOrderByClause(
#line  2439 "cs.ATG" 
out QueryExpressionOrderClause oc) {

#line  2440 "cs.ATG" 
		QueryExpressionOrdering ordering; oc = new QueryExpressionOrderClause(); oc.StartLocation = la.Location; 
		Expect(140);
		QueryExpressionOrdering(
#line  2443 "cs.ATG" 
out ordering);

#line  2443 "cs.ATG" 
		SafeAdd(oc, oc.Orderings, ordering); 
		while (la.kind == 14) {
			lexer.NextToken();
			QueryExpressionOrdering(
#line  2445 "cs.ATG" 
out ordering);

#line  2445 "cs.ATG" 
			SafeAdd(oc, oc.Orderings, ordering); 
		}

#line  2447 "cs.ATG" 
		oc.EndLocation = t.EndLocation; 
	}

	void QueryExpressionSelectClause(
#line  2460 "cs.ATG" 
out QueryExpressionSelectClause sc) {

#line  2461 "cs.ATG" 
		Expression expr; sc = new QueryExpressionSelectClause(); sc.StartLocation = la.Location; 
		Expect(133);
		Expr(
#line  2464 "cs.ATG" 
out expr);

#line  2464 "cs.ATG" 
		sc.Projection = expr; 

#line  2465 "cs.ATG" 
		sc.EndLocation = t.EndLocation; 
	}

	void QueryExpressionGroupClause(
#line  2468 "cs.ATG" 
out QueryExpressionGroupClause gc) {

#line  2469 "cs.ATG" 
		Expression expr; gc = new QueryExpressionGroupClause(); gc.StartLocation = la.Location; 
		Expect(134);
		Expr(
#line  2472 "cs.ATG" 
out expr);

#line  2472 "cs.ATG" 
		gc.Projection = expr; 
		Expect(135);
		Expr(
#line  2474 "cs.ATG" 
out expr);

#line  2474 "cs.ATG" 
		gc.GroupBy = expr; 

#line  2475 "cs.ATG" 
		gc.EndLocation = t.EndLocation; 
	}

	void QueryExpressionIntoClause(
#line  2478 "cs.ATG" 
ref QueryExpression q) {

#line  2479 "cs.ATG" 
		QueryExpression firstQuery = q;
		QueryExpression continuedQuery = new QueryExpression(); 
		continuedQuery.StartLocation = q.StartLocation;
		firstQuery.EndLocation = la.Location;
		continuedQuery.FromClause = new QueryExpressionFromClause();
		continuedQuery.FromClause.StartLocation = la.Location;
		// nest firstQuery inside continuedQuery.
		continuedQuery.FromClause.InExpression = firstQuery;
		continuedQuery.IsQueryContinuation = true;
		q = continuedQuery;
		
		Expect(136);
		Identifier();

#line  2492 "cs.ATG" 
		continuedQuery.FromClause.Identifier = t.val; 

#line  2493 "cs.ATG" 
		continuedQuery.FromClause.EndLocation = t.EndLocation; 
		QueryExpressionBody(
#line  2494 "cs.ATG" 
ref q);
	}

	void QueryExpressionOrdering(
#line  2450 "cs.ATG" 
out QueryExpressionOrdering ordering) {

#line  2451 "cs.ATG" 
		Expression expr; ordering = new QueryExpressionOrdering(); ordering.StartLocation = la.Location; 
		Expr(
#line  2453 "cs.ATG" 
out expr);

#line  2453 "cs.ATG" 
		ordering.Criteria = expr; 
		if (la.kind == 138 || la.kind == 139) {
			if (la.kind == 138) {
				lexer.NextToken();

#line  2454 "cs.ATG" 
				ordering.Direction = QueryExpressionOrderingDirection.Ascending; 
			} else {
				lexer.NextToken();

#line  2455 "cs.ATG" 
				ordering.Direction = QueryExpressionOrderingDirection.Descending; 
			}
		}

#line  2457 "cs.ATG" 
		ordering.EndLocation = t.EndLocation; 
	}


	
	public override void Parse()
	{
		CS();

	}
	
	protected override void SynErr(int line, int col, int errorNumber)
	{
		string s;
		switch (errorNumber) {
			case 0: s = "EOF expected"; break;
			case 1: s = "ident expected"; break;
			case 2: s = "Literal expected"; break;
			case 3: s = "\"=\" expected"; break;
			case 4: s = "\"+\" expected"; break;
			case 5: s = "\"-\" expected"; break;
			case 6: s = "\"*\" expected"; break;
			case 7: s = "\"/\" expected"; break;
			case 8: s = "\"%\" expected"; break;
			case 9: s = "\":\" expected"; break;
			case 10: s = "\"::\" expected"; break;
			case 11: s = "\";\" expected"; break;
			case 12: s = "\"?\" expected"; break;
			case 13: s = "\"??\" expected"; break;
			case 14: s = "\",\" expected"; break;
			case 15: s = "\".\" expected"; break;
			case 16: s = "\"{\" expected"; break;
			case 17: s = "\"}\" expected"; break;
			case 18: s = "\"[\" expected"; break;
			case 19: s = "\"]\" expected"; break;
			case 20: s = "\"(\" expected"; break;
			case 21: s = "\")\" expected"; break;
			case 22: s = "\">\" expected"; break;
			case 23: s = "\"<\" expected"; break;
			case 24: s = "\"!\" expected"; break;
			case 25: s = "\"&&\" expected"; break;
			case 26: s = "\"||\" expected"; break;
			case 27: s = "\"~\" expected"; break;
			case 28: s = "\"&\" expected"; break;
			case 29: s = "\"|\" expected"; break;
			case 30: s = "\"^\" expected"; break;
			case 31: s = "\"++\" expected"; break;
			case 32: s = "\"--\" expected"; break;
			case 33: s = "\"==\" expected"; break;
			case 34: s = "\"!=\" expected"; break;
			case 35: s = "\">=\" expected"; break;
			case 36: s = "\"<=\" expected"; break;
			case 37: s = "\"<<\" expected"; break;
			case 38: s = "\"+=\" expected"; break;
			case 39: s = "\"-=\" expected"; break;
			case 40: s = "\"*=\" expected"; break;
			case 41: s = "\"/=\" expected"; break;
			case 42: s = "\"%=\" expected"; break;
			case 43: s = "\"&=\" expected"; break;
			case 44: s = "\"|=\" expected"; break;
			case 45: s = "\"^=\" expected"; break;
			case 46: s = "\"<<=\" expected"; break;
			case 47: s = "\"->\" expected"; break;
			case 48: s = "\"=>\" expected"; break;
			case 49: s = "\"abstract\" expected"; break;
			case 50: s = "\"as\" expected"; break;
			case 51: s = "\"base\" expected"; break;
			case 52: s = "\"bool\" expected"; break;
			case 53: s = "\"break\" expected"; break;
			case 54: s = "\"byte\" expected"; break;
			case 55: s = "\"case\" expected"; break;
			case 56: s = "\"catch\" expected"; break;
			case 57: s = "\"char\" expected"; break;
			case 58: s = "\"checked\" expected"; break;
			case 59: s = "\"class\" expected"; break;
			case 60: s = "\"const\" expected"; break;
			case 61: s = "\"continue\" expected"; break;
			case 62: s = "\"decimal\" expected"; break;
			case 63: s = "\"default\" expected"; break;
			case 64: s = "\"delegate\" expected"; break;
			case 65: s = "\"do\" expected"; break;
			case 66: s = "\"double\" expected"; break;
			case 67: s = "\"else\" expected"; break;
			case 68: s = "\"enum\" expected"; break;
			case 69: s = "\"event\" expected"; break;
			case 70: s = "\"explicit\" expected"; break;
			case 71: s = "\"extern\" expected"; break;
			case 72: s = "\"false\" expected"; break;
			case 73: s = "\"finally\" expected"; break;
			case 74: s = "\"fixed\" expected"; break;
			case 75: s = "\"float\" expected"; break;
			case 76: s = "\"for\" expected"; break;
			case 77: s = "\"foreach\" expected"; break;
			case 78: s = "\"goto\" expected"; break;
			case 79: s = "\"if\" expected"; break;
			case 80: s = "\"implicit\" expected"; break;
			case 81: s = "\"in\" expected"; break;
			case 82: s = "\"int\" expected"; break;
			case 83: s = "\"interface\" expected"; break;
			case 84: s = "\"internal\" expected"; break;
			case 85: s = "\"is\" expected"; break;
			case 86: s = "\"lock\" expected"; break;
			case 87: s = "\"long\" expected"; break;
			case 88: s = "\"namespace\" expected"; break;
			case 89: s = "\"new\" expected"; break;
			case 90: s = "\"null\" expected"; break;
			case 91: s = "\"object\" expected"; break;
			case 92: s = "\"operator\" expected"; break;
			case 93: s = "\"out\" expected"; break;
			case 94: s = "\"override\" expected"; break;
			case 95: s = "\"params\" expected"; break;
			case 96: s = "\"private\" expected"; break;
			case 97: s = "\"protected\" expected"; break;
			case 98: s = "\"public\" expected"; break;
			case 99: s = "\"readonly\" expected"; break;
			case 100: s = "\"ref\" expected"; break;
			case 101: s = "\"return\" expected"; break;
			case 102: s = "\"sbyte\" expected"; break;
			case 103: s = "\"sealed\" expected"; break;
			case 104: s = "\"short\" expected"; break;
			case 105: s = "\"sizeof\" expected"; break;
			case 106: s = "\"stackalloc\" expected"; break;
			case 107: s = "\"static\" expected"; break;
			case 108: s = "\"string\" expected"; break;
			case 109: s = "\"struct\" expected"; break;
			case 110: s = "\"switch\" expected"; break;
			case 111: s = "\"this\" expected"; break;
			case 112: s = "\"throw\" expected"; break;
			case 113: s = "\"true\" expected"; break;
			case 114: s = "\"try\" expected"; break;
			case 115: s = "\"typeof\" expected"; break;
			case 116: s = "\"uint\" expected"; break;
			case 117: s = "\"ulong\" expected"; break;
			case 118: s = "\"unchecked\" expected"; break;
			case 119: s = "\"unsafe\" expected"; break;
			case 120: s = "\"ushort\" expected"; break;
			case 121: s = "\"using\" expected"; break;
			case 122: s = "\"virtual\" expected"; break;
			case 123: s = "\"void\" expected"; break;
			case 124: s = "\"volatile\" expected"; break;
			case 125: s = "\"while\" expected"; break;
			case 126: s = "\"partial\" expected"; break;
			case 127: s = "\"where\" expected"; break;
			case 128: s = "\"get\" expected"; break;
			case 129: s = "\"set\" expected"; break;
			case 130: s = "\"add\" expected"; break;
			case 131: s = "\"remove\" expected"; break;
			case 132: s = "\"yield\" expected"; break;
			case 133: s = "\"select\" expected"; break;
			case 134: s = "\"group\" expected"; break;
			case 135: s = "\"by\" expected"; break;
			case 136: s = "\"into\" expected"; break;
			case 137: s = "\"from\" expected"; break;
			case 138: s = "\"ascending\" expected"; break;
			case 139: s = "\"descending\" expected"; break;
			case 140: s = "\"orderby\" expected"; break;
			case 141: s = "\"let\" expected"; break;
			case 142: s = "\"join\" expected"; break;
			case 143: s = "\"on\" expected"; break;
			case 144: s = "\"equals\" expected"; break;
			case 145: s = "??? expected"; break;
			case 146: s = "invalid NamespaceMemberDecl"; break;
			case 147: s = "invalid NonArrayType"; break;
			case 148: s = "invalid Identifier"; break;
			case 149: s = "invalid AttributeArguments"; break;
			case 150: s = "invalid Expr"; break;
			case 151: s = "invalid TypeModifier"; break;
			case 152: s = "invalid TypeDecl"; break;
			case 153: s = "invalid TypeDecl"; break;
			case 154: s = "this symbol not expected in ClassBody"; break;
			case 155: s = "this symbol not expected in InterfaceBody"; break;
			case 156: s = "invalid IntegralType"; break;
			case 157: s = "invalid FormalParameterList"; break;
			case 158: s = "invalid FormalParameterList"; break;
			case 159: s = "invalid ClassType"; break;
			case 160: s = "invalid ClassMemberDecl"; break;
			case 161: s = "invalid ClassMemberDecl"; break;
			case 162: s = "invalid StructMemberDecl"; break;
			case 163: s = "invalid StructMemberDecl"; break;
			case 164: s = "invalid StructMemberDecl"; break;
			case 165: s = "invalid StructMemberDecl"; break;
			case 166: s = "invalid StructMemberDecl"; break;
			case 167: s = "invalid StructMemberDecl"; break;
			case 168: s = "invalid StructMemberDecl"; break;
			case 169: s = "invalid StructMemberDecl"; break;
			case 170: s = "invalid StructMemberDecl"; break;
			case 171: s = "invalid StructMemberDecl"; break;
			case 172: s = "invalid StructMemberDecl"; break;
			case 173: s = "invalid StructMemberDecl"; break;
			case 174: s = "invalid StructMemberDecl"; break;
			case 175: s = "invalid InterfaceMemberDecl"; break;
			case 176: s = "invalid InterfaceMemberDecl"; break;
			case 177: s = "invalid InterfaceMemberDecl"; break;
			case 178: s = "invalid TypeWithRestriction"; break;
			case 179: s = "invalid TypeWithRestriction"; break;
			case 180: s = "invalid SimpleType"; break;
			case 181: s = "invalid AccessorModifiers"; break;
			case 182: s = "this symbol not expected in Block"; break;
			case 183: s = "invalid EventAccessorDecls"; break;
			case 184: s = "invalid ConstructorInitializer"; break;
			case 185: s = "invalid OverloadableOperator"; break;
			case 186: s = "invalid AccessorDecls"; break;
			case 187: s = "invalid InterfaceAccessors"; break;
			case 188: s = "invalid InterfaceAccessors"; break;
			case 189: s = "invalid GetAccessorDecl"; break;
			case 190: s = "invalid SetAccessorDecl"; break;
			case 191: s = "invalid VariableInitializer"; break;
			case 192: s = "this symbol not expected in Statement"; break;
			case 193: s = "invalid Statement"; break;
			case 194: s = "invalid AssignmentOperator"; break;
			case 195: s = "invalid ObjectPropertyInitializerOrVariableInitializer"; break;
			case 196: s = "invalid ObjectPropertyInitializerOrVariableInitializer"; break;
			case 197: s = "invalid EmbeddedStatement"; break;
			case 198: s = "invalid EmbeddedStatement"; break;
			case 199: s = "this symbol not expected in EmbeddedStatement"; break;
			case 200: s = "invalid EmbeddedStatement"; break;
			case 201: s = "invalid ForInitializer"; break;
			case 202: s = "invalid GotoStatement"; break;
			case 203: s = "invalid TryStatement"; break;
			case 204: s = "invalid ResourceAcquisition"; break;
			case 205: s = "invalid SwitchLabel"; break;
			case 206: s = "invalid CatchClauses"; break;
			case 207: s = "invalid UnaryExpr"; break;
			case 208: s = "invalid PrimaryExpr"; break;
			case 209: s = "invalid PrimaryExpr"; break;
			case 210: s = "invalid PrimaryExpr"; break;
			case 211: s = "invalid TypeArgumentList"; break;
			case 212: s = "invalid NewExpression"; break;
			case 213: s = "invalid NewExpression"; break;
			case 214: s = "invalid LambdaExpressionParameter"; break;
			case 215: s = "invalid LambdaExpressionBody"; break;
			case 216: s = "invalid RelationalExpr"; break;
			case 217: s = "invalid RelationalExpr"; break;
			case 218: s = "invalid TypeParameterConstraintsClauseBase"; break;
			case 219: s = "invalid QueryExpressionBody"; break;

			default: s = "error " + errorNumber; break;
		}
		this.Errors.Error(line, col, s);
	}
	
	private bool StartOf(int s)
	{
		return set[s, lexer.LookAhead.kind];
	}
	
	static bool[,] set = {
	{T,T,T,x, T,T,T,x, x,x,x,T, x,x,x,x, T,T,T,x, T,x,x,x, T,x,x,T, T,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,T, T,T,T,x, x,T,T,T, T,T,T,T, T,T,T,x, T,T,T,T, T,x,T,T, T,T,T,T, T,x,T,T, T,x,T,T, x,T,T,T, x,x,T,x, T,T,T,T, x,T,T,T, T,T,x,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,x,x,x, T,T,x,x, x,x,x,x, T,T,T,x, x,x,x,T, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
	{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,x,x,x, x,T,x,x, x,x,x,x, T,T,T,x, x,x,x,T, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
	{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,x,x,x, T,T,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
	{x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,T,x, x,T,x,x, x,x,T,x, x,x,T,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,T,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, T,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
	{x,T,T,x, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, T,x,x,T, T,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,x,T,x, x,T,T,x, x,x,T,T, T,x,T,x, x,x,x,x, T,x,x,T, x,x,x,x, x,x,T,x, x,x,x,T, x,T,T,T, x,x,x,x, x,x,x,x, x,x,T,x, T,T,x,x, T,x,x,T, x,T,x,T, T,T,T,x, T,x,x,T, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, T,T,T,T, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
	{x,T,x,T, T,T,T,T, T,T,x,T, T,T,T,T, T,T,T,T, T,T,T,T, x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x,T,T,x, T,x,T,x, x,T,x,T, T,x,T,x, T,x,T,x, T,T,T,T, x,x,T,T, x,x,x,x, T,x,T,T, T,T,x,T, x,T,x,T, x,x,T,x, T,T,T,T, x,x,T,T, T,x,x,T, T,T,x,x, x,x,x,x, T,T,x,T, T,x,T,T, T,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
	{x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,T,x, x,T,x,x, x,x,T,x, x,x,T,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, x,x,T,x, T,x,x,x, T,x,x,x, x,x,x,x, T,T,x,x, T,x,x,T, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,T,x, x,T,x,x, x,x,T,x, x,x,T,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,T,x, x,x,x,T, x,x,x,T, x,T,x,T, x,x,x,x, T,x,T,x, T,x,x,x, T,x,x,x, x,x,x,x, T,T,x,x, T,x,x,T, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, T,x,T,x, x,T,x,T, T,x,T,x, T,x,T,x, T,T,T,T, x,x,T,T, x,x,x,x, T,x,T,T, T,x,x,T, x,T,x,T, x,x,T,x, T,T,T,T, x,x,T,T, T,x,x,T, T,T,x,x, x,x,x,x, T,T,x,T, T,x,T,T, T,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, T,x,T,x, x,T,x,T, T,x,T,x, T,x,T,x, T,T,T,T, x,x,T,T, x,x,x,x, T,x,T,T, T,x,x,T, x,T,x,T, x,x,T,x, T,T,T,T, x,x,T,T, T,x,x,T, T,T,x,x, x,x,x,x, T,T,x,T, T,x,T,T, T,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, T,x,T,x, x,T,x,T, T,x,T,x, T,x,T,x, T,T,T,T, x,x,T,T, x,x,x,x, T,x,T,T, T,x,x,T, x,T,x,T, x,x,T,x, T,T,T,T, x,x,T,T, T,x,x,T, T,T,x,x, x,x,x,x, T,T,x,T, T,x,T,T, T,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,T,x, x,T,x,x, x,x,T,x, x,x,T,x, x,T,x,x, x,x,x,T, x,x,x,x, x,x,T,x, x,x,x,T, x,T,x,T, x,x,x,x, x,x,x,x, x,x,T,x, T,x,x,x, T,x,x,x, x,x,x,x, T,T,x,x, T,x,x,T, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,T,x, x,T,x,x, x,x,T,x, x,x,T,x, x,T,x,x, x,x,x,T, x,x,x,x, x,x,T,x, x,x,x,T, x,T,x,T, x,x,x,x, x,x,x,x, x,x,T,x, T,x,x,x, T,x,x,x, x,x,x,x, T,T,x,x, T,x,x,T, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,T,x, x,T,x,x, x,x,T,x, x,x,T,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,T,x, x,x,x,T, x,x,x,T, x,T,x,x, x,x,x,x, T,x,T,x, T,x,x,x, T,x,x,x, x,x,x,x, T,T,x,x, T,x,x,T, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,T,x, x,x,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,x,T,x, T,T,T,T, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,T, x,x,T,x, T,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
	{x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,T,x, x,T,x,T, T,x,T,x, T,x,T,x, T,T,T,x, x,x,x,T, x,x,x,x, T,x,T,T, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, x,x,T,x, T,x,x,x, T,T,x,x, x,x,x,x, T,T,x,x, T,x,x,T, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
	{x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,T,x, x,T,x,x, x,x,T,x, x,x,T,x, x,T,x,x, x,x,x,T, x,x,x,x, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, x,x,T,x, T,x,x,x, T,x,x,x, x,x,x,x, T,T,x,x, T,x,x,T, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, T,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
	{x,T,T,x, T,T,T,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, T,x,x,T, T,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,T,T,x, x,T,T,x, T,T,T,T, T,T,T,x, x,x,x,x, T,x,T,T, T,T,T,T, x,x,T,x, x,x,T,T, x,T,T,T, x,x,x,x, x,x,x,x, x,T,T,x, T,T,x,x, T,x,T,T, T,T,T,T, T,T,T,T, T,T,x,T, x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,T,T,x, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, T,x,x,T, T,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,x,T,x, x,T,T,x, x,x,T,T, T,x,T,x, x,x,x,x, T,x,x,T, x,x,x,x, x,x,T,x, x,x,x,T, x,T,T,T, x,T,x,x, x,x,x,x, T,x,T,x, T,T,x,x, T,x,x,T, x,T,x,T, T,T,T,x, T,x,x,T, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
	{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
	{T,T,T,x, T,T,T,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, T,x,x,T, T,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,T,T,x, x,T,T,x, T,T,T,T, T,T,T,x, x,x,x,x, T,x,T,T, T,T,T,T, x,x,T,x, x,x,T,T, x,T,T,T, x,x,x,x, x,x,x,x, x,T,T,x, T,T,x,x, T,x,T,T, T,T,T,T, T,T,T,T, T,T,x,T, x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,T,T,x, T,T,T,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, T,x,x,T, T,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,T,T,x, x,T,T,x, x,T,T,T, T,T,T,x, x,x,x,x, T,x,T,T, T,T,T,T, x,x,T,x, x,x,T,T, x,T,T,T, x,x,x,x, x,x,x,x, x,T,T,x, T,T,x,x, T,x,T,T, T,T,T,T, T,T,T,T, T,T,x,T, x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,T,T,x, T,T,T,x, x,x,x,x, x,x,x,x, T,x,x,x, T,x,x,x, T,x,x,T, T,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,x,T,x, x,T,T,x, x,x,T,T, T,x,T,x, x,x,x,x, T,x,x,T, x,x,x,x, x,x,T,x, x,x,x,T, x,T,T,T, x,x,x,x, x,x,x,x, x,x,T,x, T,T,T,x, T,x,x,T, x,T,x,T, T,T,T,x, T,x,x,T, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,T,T,x, T,T,T,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, T,x,x,T, T,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,T,T,T, x,T,T,x, T,T,T,T, T,T,T,x, x,x,x,x, T,x,T,T, T,T,T,T, x,x,T,x, x,x,T,T, x,T,T,T, x,x,x,x, x,x,x,x, x,T,T,x, T,T,x,x, T,x,T,T, T,T,T,T, T,T,T,T, T,T,x,T, x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,x,x,x, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,T, T,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
	{x,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,x,T,x, x,T,T,x, x,x,T,T, T,x,T,x, x,x,x,x, T,x,x,T, x,x,x,x, x,x,T,x, x,x,x,T, x,T,T,T, x,x,x,x, x,x,x,x, x,x,T,x, T,T,x,x, T,x,x,T, x,T,x,T, T,T,T,x, T,x,x,T, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
	{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,T,x, x,T,x,x, x,x,T,x, x,x,T,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, x,x,T,x, T,x,x,x, T,x,x,x, x,x,x,x, T,T,x,x, T,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
	{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,T,x, T,x,x,x, x,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
	{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, x,x,x,x, x,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
	{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, x,x,x,x, x,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
	{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,T,x,x, T,T,T,x, x,x,x}

	};
} // end Parser

}