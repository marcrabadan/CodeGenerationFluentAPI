using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CodeGeneration.FluentApi.Translator
{
    public class ExpressionToCodeObject
    {
        #region .: Variables :. 

        private readonly Expression _expression;
        private readonly Dictionary<string, CodeTypeMember> _members;

        #endregion

        #region .: Constructor :.

        public ExpressionToCodeObject(Expression expression)
        {
            _expression = expression;
            _members = new Dictionary<string, CodeTypeMember>();
        }

        #endregion

        #region .: Public Methods :.

        public IEnumerable<CodeTypeMember> Translate()
        {
            try
            {
                CodeObject result = DoTranslation(_expression);
                return _members.Select(c => c.Value).ToList();
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error at Translate(): {0}", ex.Message);
                return null;
            }
        }

        #endregion
        
        
        #region .: Private Methods :.

        private CodeObject DoTranslation(Expression exp)
        {
            if (exp == null)
                return null;
            switch (exp.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    return TranslateUnary((UnaryExpression)exp);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                    return TranslateBinary((BinaryExpression)exp);
                case ExpressionType.TypeIs:
                    return TranslateTypeIs((TypeBinaryExpression)exp);
                case ExpressionType.Conditional:
                    return TranslateConditional((ConditionalExpression)exp);
                case ExpressionType.Constant:
                    return TranslateConstant((ConstantExpression)exp);
                case ExpressionType.Parameter:
                    return TranslateParameter((ParameterExpression)exp);
                case ExpressionType.MemberAccess:
                    return TranslateMemberAccess((MemberExpression)exp);
                case ExpressionType.Call:
                    return TranslateMethodCall((MethodCallExpression)exp);
                case ExpressionType.Lambda:
                    return TranslateLambda((LambdaExpression)exp);
                case ExpressionType.New:
                    return TranslateNew((NewExpression)exp);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return TranslateNewArray((NewArrayExpression)exp);
                case ExpressionType.Invoke:
                    return TranslateInvocation((InvocationExpression)exp);
                case ExpressionType.MemberInit:
                    return TranslateMemberInit((MemberInitExpression)exp);
                case ExpressionType.ListInit:
                    return TranslateListInit((ListInitExpression)exp);
                default:
                    throw new Exception(string.Format("Unhandled expression type: '{0}'", exp.NodeType));
            }
        }

        private CodeObject TranslateBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return TranslateMemberAssignment((MemberAssignment)binding);
                case MemberBindingType.MemberBinding:
                    return TranslateMemberMemberBinding((MemberMemberBinding)binding);
                case MemberBindingType.ListBinding:
                    return TranslateMemberListBinding((MemberListBinding)binding);
                default:
                    throw new Exception(string.Format("Unhandled binding type '{0}'", binding.BindingType));
            }
        }

        private CodeExpression TranslateElementInitializer(ElementInit initializer)
        {
            ReadOnlyCollection<CodeExpression> arguments = TranslateExpressionList(initializer.Arguments);

            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), initializer.AddMethod.Name), arguments.ToArray());
        }

        private CodeObject TranslateUnary(UnaryExpression u)
        {
            CodeObject operand = DoTranslation(u.Operand);

            return operand;
        }

        private CodeBinaryOperatorType BindOperant(ExpressionType e)
        {
            switch (e)
            {
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return CodeBinaryOperatorType.Add;
                case ExpressionType.And:
                    return CodeBinaryOperatorType.BitwiseAnd;
                case ExpressionType.AndAlso:
                    return CodeBinaryOperatorType.BooleanAnd;
                case ExpressionType.Or:
                    return CodeBinaryOperatorType.BitwiseOr;
                case ExpressionType.OrElse:
                    return CodeBinaryOperatorType.BooleanOr;
                case ExpressionType.ExclusiveOr:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Coalesce:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.Equal:
                    return CodeBinaryOperatorType.IdentityEquality;
                case ExpressionType.NotEqual:
                    return CodeBinaryOperatorType.IdentityInequality;
                case ExpressionType.GreaterThan:
                    return CodeBinaryOperatorType.GreaterThan;
                case ExpressionType.GreaterThanOrEqual:
                    return CodeBinaryOperatorType.GreaterThanOrEqual;
                case ExpressionType.LessThan:
                    return CodeBinaryOperatorType.LessThan;
                case ExpressionType.LessThanOrEqual:
                    return CodeBinaryOperatorType.LessThanOrEqual;
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return CodeBinaryOperatorType.Multiply;
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return CodeBinaryOperatorType.Subtract;
                case ExpressionType.Power:
                case ExpressionType.Divide:
                    return CodeBinaryOperatorType.Divide;
                case ExpressionType.Modulo:
                    return CodeBinaryOperatorType.Modulus;
                default:
                    throw new Exception("No support!");
            }
        }

        private CodeBinaryOperatorExpression TranslateBinary(BinaryExpression b)
        {
            var left = DoTranslation(b.Left) as CodeExpression;
            var right = DoTranslation(b.Right) as CodeExpression;
            CodeObject conversion = DoTranslation(b.Conversion);

            CodeBinaryOperatorType operant = BindOperant(b.NodeType);
            var condExpr = new CodeBinaryOperatorExpression(left, operant, right);
            return condExpr;
        }

        private CodeObject TranslateTypeIs(TypeBinaryExpression b)
        {
            CodeObject expr = DoTranslation(b.Expression);
            return expr;
        }

        private CodeExpression TranslateConstant(ConstantExpression c)
        {
            if (c.Value == null)
            {
                return new CodePrimitiveExpression(null);
            }
            else if (c.Value.GetType().IsValueType || c.Value is string)
            {
                return new CodePrimitiveExpression(c.Value);
            }
            else
            {
                return new CodeVariableReferenceExpression(c.Value.ToString());
            }
        }

        private CodeObject TranslateConditional(ConditionalExpression c)
        {
            CodeObject test = DoTranslation(c.Test);
            CodeExpression ifTrue = DoTranslation(c.IfTrue) as CodeExpression;
            CodeExpression ifFalse = DoTranslation(c.IfFalse) as CodeExpression;

            var ifStatement = new CodeConditionStatement(test as CodeExpression,
                                                         new CodeStatement[] { new CodeExpressionStatement(ifTrue) },
                                                         new CodeStatement[] { new CodeExpressionStatement(ifFalse) });
            return ifStatement;
        }

        private CodeObject TranslateParameter(ParameterExpression p)
        {
            return new CodeArgumentReferenceExpression(p.Name);
        }

        private CodeObject TranslateMemberAccess(MemberExpression m)
        {

            CodeObject exp = DoTranslation(m.Expression);

            if (exp is CodePrimitiveExpression)
            {
                return exp;
            }
            else
            {
                Type memType;
                if (m.Member.MemberType == MemberTypes.Field)
                    memType = (m.Member as FieldInfo).FieldType;
                else memType = (m.Member as PropertyInfo).PropertyType;


                _members[m.Member.Name] = new CodeMemberField(memType, m.Member.Name);
                return new CodeVariableReferenceExpression(m.Member.Name);
            }
        }

        private CodeObject TranslateMethodCall(MethodCallExpression m)
        {
            CodeObject obj = DoTranslation(m.Object);
            IEnumerable<CodeExpression> args = TranslateExpressionList(m.Arguments);

            if (obj == null)
            {
                return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(m.Method.DeclaringType), m.Method.Name, args.ToArray());
            }
            else
            {
                return new CodeMethodInvokeExpression(obj as CodeExpression, m.Method.Name, args.ToArray());
            }
        }

        private ReadOnlyCollection<CodeExpression> TranslateExpressionList(ReadOnlyCollection<Expression> expressions)
        {
            List<CodeExpression> list = new List<CodeExpression>();
            for (int i = 0, n = expressions.Count; i < n; i++)
            {
                CodeExpression p = (CodeExpression)DoTranslation(expressions[i]);
                list.Add(p);
            }
            return list.AsReadOnly();
        }

        private CodeExpression TranslateMemberAssignment(MemberAssignment assignment)
        {

            CodeObject e = DoTranslation(assignment.Expression);
            return e as CodeExpression;

        }

        private CodeObject TranslateMemberMemberBinding(MemberMemberBinding binding)
        {

            IEnumerable<CodeExpression> bindings = TranslateBindingList(binding.Bindings) as IEnumerable<CodeExpression>;
            return new CodeObjectCreateExpression(binding.Member.Name, bindings.ToArray());
        }

        private CodeObject TranslateMemberListBinding(MemberListBinding binding)
        {

            IEnumerable<CodeExpression> initializers = TranslateElementInitializerList(binding.Initializers);

            return new CodeObjectCreateExpression(binding.Member.Name, initializers.ToArray());

        }

        private IEnumerable<CodeExpression> TranslateBindingList(ReadOnlyCollection<MemberBinding> original)
        {
            List<CodeExpression> list = new List<CodeExpression>();
            for (int i = 0, n = original.Count; i < n; i++)
            {
                CodeExpression b = TranslateBinding(original[i]) as CodeExpression;

                list.Add(b);

            }
            return list;
        }

        private IEnumerable<CodeExpression> TranslateElementInitializerList(ReadOnlyCollection<ElementInit> original)
        {
            List<CodeExpression> list = new List<CodeExpression>();
            for (int i = 0, n = original.Count; i < n; i++)
            {
                CodeExpression init = TranslateElementInitializer(original[i]);

                list.Add(init);

            }

            return list;
        }

        private CodeMethodReferenceExpression TranslateLambda(LambdaExpression lambda)
        {
            var body = DoTranslation(lambda.Body);
            var lambdaMethod = new CodeMemberMethod();

            lambdaMethod.Name = lambda.Type.Name;
            if (lambdaMethod.Name.Contains("Func"))
                lambdaMethod.ReturnType = new CodeTypeReference(lambda.Body.Type);

            foreach (var item in lambda.Parameters)
            {
                lambdaMethod.Parameters.Add(new CodeParameterDeclarationExpression(item.Type, item.Name));
            }

            if (body is CodeExpression)
            {
                if (lambdaMethod.ReturnType.BaseType.Contains("Void"))
                    lambdaMethod.Statements.Add((body as CodeExpression));

                else
                    lambdaMethod.Statements.Add(new CodeMethodReturnStatement(body as CodeExpression));
            }
            else if (body is CodeStatement)
            {
                lambdaMethod.Statements.Add((body as CodeStatement));
            }

            _members[lambda.Type.FullName] = lambdaMethod;
            return new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), lambdaMethod.Name);
        }

        private CodeObject TranslateNew(NewExpression nex)
        {
            IEnumerable<CodeExpression> args = TranslateExpressionList(nex.Arguments);


            return new CodeObjectCreateExpression(nex.Type.Name, args.ToArray());

        }

        private CodeObject TranslateMemberInit(MemberInitExpression init)
        {
            CodeObject n = TranslateNew(init.NewExpression);
            CodeExpression[] bindings = TranslateBindingList(init.Bindings).ToArray();

            for (int i = 0; i < init.Bindings.Count; i++)
            {
                var assignProperty = new CodeAssignStatement(new CodePropertyReferenceExpression(n as CodeExpression, init.Bindings[i].Member.Name), bindings[i]);
            }

            return n;
        }

        private CodeObject TranslateListInit(ListInitExpression init)
        {
            CodeObject n = TranslateNew(init.NewExpression);
            IEnumerable<CodeExpression> initializers = TranslateElementInitializerList(init.Initializers);

            return n;
        }

        private CodeObject TranslateNewArray(NewArrayExpression na)
        {
            IEnumerable<CodeExpression> exprs = TranslateExpressionList(na.Expressions);

            return new CodeArrayCreateExpression(new CodeTypeReference(na.Type), exprs.ToArray());
        }

        private CodeObject TranslateInvocation(InvocationExpression iv)
        {
            IEnumerable<CodeExpression> args = TranslateExpressionList(iv.Arguments);

            var expr = DoTranslation(iv.Expression) as CodeExpression;

            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(expr, "Method"), args.ToArray());
        }  

        #endregion

        
            
    }
}
