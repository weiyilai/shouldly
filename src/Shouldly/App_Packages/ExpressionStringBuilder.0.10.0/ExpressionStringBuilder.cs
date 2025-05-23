﻿

// ReSharper disable CheckNamespace
namespace ExpressionToString;

class ExpressionStringBuilder : ExpressionVisitor
{
    // ReSharper disable InconsistentNaming
    private readonly StringBuilder builder = new();
    private readonly bool trimLongArgumentList;
    private bool skipDot;

    private ExpressionStringBuilder(bool trimLongArgumentList)
    {
        this.trimLongArgumentList = trimLongArgumentList;
    }

    /// <summary>
    /// A nicely formatted ToString of an expression
    /// </summary>
    /// <param name="expression">The expression to format</param>
    /// <param name="trimLongArgumentList">If true will replace large (>3) argument lists with an ellipsis</param>
    /// <returns></returns>
    public static string ToString(Expression? expression, bool trimLongArgumentList = false)
    {
        var visitor = new ExpressionStringBuilder(trimLongArgumentList);
        visitor.Visit(expression);
        var s = visitor.builder.ToString();
        return s;
    }

    protected override Expression VisitLambda<T>(Expression<T> node)
    {
        if (node.Parameters.Any())
        {
            Out("(");
            Out(string.Join(",", node.Parameters.Select(n => n.Name)));
            Out(") => ");
        }

        Visit(node.Body);
        return node;
    }

    protected override Expression VisitInvocation(InvocationExpression node)
    {
        var visitInvocation = base.VisitInvocation(node);
        Out("()");
        return visitInvocation;
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        Out("(");
        Visit(node.Left);
        Out(" ");
        Out(ToString(node.NodeType));
        Out(" ");
        Visit(node.Right);
        Out(")");
        return node;
    }

    protected override Expression VisitTypeBinary(TypeBinaryExpression node)
    {
        Out(node.ToString());
        return node;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        Out(node.Name);
        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Expression?.NodeType == ExpressionType.Constant)
        {
            Visit(node.Expression);
            if (skipDot)
            {
                skipDot = false;
                Out(node.Member.Name);
            }
            else
            {
                Out("." + node.Member.Name);
            }
        }
        else
        {
            Visit(node.Expression);
            Out("." + node.Member.Name);
        }

        return node;
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (CheckIfAnonymousType(node.Type))
        {
            skipDot = true;
            return node;
        }

        if (node.Value == null)
        {
            Out("null");
        }
        else
        {
            if (node.Value is string stringValue)
            {
                Out("\"" + stringValue + "\"");
            }
            else
            {
                if (node.Value is bool)
                {
                    Out(node.Value.ToString()?.ToLower());
                }
                else
                {
                    var valueToString = node.Value.ToString();
                    var type = node.Value.GetType();
                    if (type.FullName != valueToString)
                    {
                        Out(valueToString);
                    }
                    else
                    {
                        skipDot = true;
                    }
                }
            }
        }

        return node;
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        if (node.NodeType == ExpressionType.Convert)
        {
            Visit(node.Operand);
            return node;
        }

        if (node.NodeType == ExpressionType.Not)
        {
            Out("!");
            Visit(node.Operand);
            return node;
        }

        if (node.NodeType == ExpressionType.TypeAs)
        {
            Out("(");
            Visit(node.Operand);
            Out(" As " + node.Type.Name + ")");
            return node;
        }

        return base.VisitUnary(node);
    }

    protected override Expression VisitNew(NewExpression node)
    {
        Out("new " + node.Type.Name + "(");
        VisitArguments(node.Arguments.ToArray());
        Out(")");
        return node;
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        Visit(node.Object);

        if (!skipDot && !node.Method.IsStatic)
        {
            Out(".");
            skipDot = false;
        }

        Out(node.Method.Name + "(");
        var args = node.Arguments.ToArray();
        if (args.Length > 3 && trimLongArgumentList)
        {
            Out("...");
        }
        else
        {
            VisitArguments(args);
        }

        Out(")");
        return node;
    }

    protected override Expression VisitConditional(ConditionalExpression node)
    {
        Out("IIF(");
        Visit(node.Test);
        Out(", ");
        Visit(node.IfTrue);
        Out(", ");
        Visit(node.IfFalse);
        Out(")");
        return node;
    }

    private void VisitArguments(Expression[] arguments)
    {
        var argindex = 0;
        while (argindex < arguments.Length)
        {
            Visit(arguments[argindex]);
            argindex++;

            if (argindex < arguments.Length)
            {
                Out(", ");
            }
        }
    }

    private static bool CheckIfAnonymousType(Type type)
    {
        // hack: the only way to detect anonymous types right now
        var isDefined = type.IsDefined(typeof(CompilerGeneratedAttribute), false);
        return isDefined
               && (type.IsGenericType && type.Name.Contains("AnonymousType") || type.Name.Contains("DisplayClass"))
               && (type.Name.StartsWith("<>", StringComparison.Ordinal) || type.Name.StartsWith("VB$", StringComparison.Ordinal));
    }

    private static string ToString(ExpressionType type)
    {
        switch (type)
        {
            case ExpressionType.Add:
                return "+";
            case ExpressionType.And:
                return "&";
            case ExpressionType.AndAlso:
                return "&&";
            case ExpressionType.Divide:
                return "/";
            case ExpressionType.Equal:
                return "==";
            case ExpressionType.GreaterThan:
                return ">";
            case ExpressionType.GreaterThanOrEqual:
                return ">=";
            case ExpressionType.LessThan:
                return "<";
            case ExpressionType.LessThanOrEqual:
                return "<=";
            case ExpressionType.Modulo:
                return "%";
            case ExpressionType.Multiply:
                return "*";
            case ExpressionType.Negate:
                return "-";
            case ExpressionType.Not:
                return "!";
            case ExpressionType.NotEqual:
                return "!=";
            case ExpressionType.Or:
                return "|";
            case ExpressionType.OrElse:
                return "||";
            case ExpressionType.Subtract:
                return "-";
            case ExpressionType.Coalesce:
                return "??";
            case ExpressionType.ExclusiveOr:
                return "^";
            default:
                throw new NotImplementedException();
        }
    }

    private void Out(string? s)
    {
        builder.Append(s);
    }
}