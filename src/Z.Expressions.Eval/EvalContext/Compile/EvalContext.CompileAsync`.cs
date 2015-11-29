﻿// Description: C# Expression Evaluator | Evaluate, Compile and Execute C# code and expression at runtime.
// Website & Documentation: https://github.com/zzzprojects/Eval-Expression.NET
// Forum: https://zzzprojects.uservoice.com/forums/327759-eval-expression-net
// License: http://www.zzzprojects.com/license-agreement/
// More projects: http://www.zzzprojects.com/
// Copyright (c) 2015 ZZZ Projects. All rights reserved.

#if NET45
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Z.Expressions
{
    public partial class EvalContext
    {
        /// <summary>Compile the code or expression and return a TDelegate of type Func or Action to execute.</summary>
        /// <typeparam name="TDelegate">Type of the delegate (Func or Action) to use to compile the code or expression.</typeparam>
        /// <param name="code">The code or expression to compile.</param>
        /// <returns>A TDelegate of type Func or Action that represents the compiled code or expression.</returns>
        public Task<TDelegate> CompileAsync<TDelegate>(string code)
        {
            return CompileAsync<TDelegate>(code, null);
        }

        /// <summary>Compile the code or expression and return a TDelegate of type Func or Action to execute.</summary>
        /// <typeparam name="TDelegate">Type of the delegate (Func or Action) to use to compile the code or expression.</typeparam>
        /// <param name="code">The code or expression to compile.</param>
        /// <param name="parameterNames">Parameter names used to compile the code or expressions.</param>
        /// <returns>A TDelegate of type Func or Action that represents the compiled code or expression.</returns>
        public Task<TDelegate> CompileAsync<TDelegate>(string code, IEnumerable<string> parameterNames)
        {
            return CompileAsync<TDelegate>(code, parameterNames.ToArray());
        }

        /// <summary>Compile the code or expression and return a TDelegate of type Func or Action to execute.</summary>
        /// <typeparam name="TDelegate">Type of the delegate (Func or Action) to use to compile the code or expression.</typeparam>
        /// <param name="code">The code or expression to compile.</param>
        /// <param name="parameterNames">Parameter names used to compile the code or expressions.</param>
        /// <returns>A TDelegate of type Func or Action that represents the compiled code or expression.</returns>
        public Task<TDelegate> CompileAsync<TDelegate>(string code, params string[] parameterNames)
        {
            var parameterTypes = new Dictionary<string, Type>();

            var tDelegate = typeof (TDelegate);
            var arguments = tDelegate.GetGenericArguments();
            var isAction = tDelegate.FullName.StartsWith("System.Action");

            var tReturn = isAction ? null : arguments.Last();
            var lastArgumentPosition = isAction ? arguments.Length : arguments.Length - 1;

            for (var i = 0; i < lastArgumentPosition; i++)
            {
                if (parameterNames != null && i <= parameterNames.Length)
                {
                    parameterTypes.Add(parameterNames[i], arguments[i]);
                }
                else
                {
                    parameterTypes.Add(string.Concat("{", i, "}"), arguments[i]);
                }
            }

            return EvalCompiler.CompileAsync<TDelegate>(this, code, parameterTypes, tReturn, EvalCompilerParameterKind.Typed);
        }
    }
}

#endif