﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using LinFu.AOP.Cecil.Interfaces;

namespace LinFu.AOP.Cecil
{
    /// <summary>
    /// An extension class that adds support for intercepting the 'new' operator with LinFu.AOP.
    /// </summary>
    public static class NewOperatorInterceptionExtensions
    {
        /// <summary>
        /// Modifies a <paramref name="target"/> assembly to support intercepting calls to the 'new' operator.
        /// </summary>
        /// <param name="target">The assembly to be modified.</param>
        /// <param name="typeFilter">The functor that determines which type instantiations should be intercepted.</param>
        /// <param name="methodFilter">The filter that determines which host methods will be modified</param>
        /// <remarks>
        /// The type filter determines which concrete types and constructors should be intercepted at runtime.
        /// For example, the following functor code intercepts types named "Foo":
        /// <code>
        ///     Func&lt;MethodReference, TypeReference, MethodReference&gt; filter = 
        ///     (constructor, concreteType, hostMethod) => concreteType.Name == "Foo";
        /// </code>
        /// </remarks>
        public static void InterceptNewInstances(this AssemblyDefinition target, Func<MethodReference, TypeReference, MethodReference, bool> typeFilter,
            Func<MethodReference, bool> methodFilter)
        {
            var redirector = new RedirectNewInstancesToActivator(typeFilter);
            target.InterceptNewInstancesWith(redirector, methodFilter);
        }

        /// <summary>
        /// Modifies a <paramref name="target"/> method to support intercepting calls to the 'new' operator.
        /// </summary>
        /// <param name="target">The item to be modified.</param>
        /// <param name="typeFilter">The functor that determines which type instantiations should be intercepted.</param>
        /// <param name="methodFilter">The filter that determines which host methods will be modified</param>
        /// <remarks>
        /// The type filter determines which concrete types and constructors should be intercepted at runtime.
        /// For example, the following functor code intercepts types named "Foo":
        /// <code>
        ///     Func&lt;MethodReference, TypeReference, MethodReference&gt; filter = 
        ///     (constructor, concreteType, hostMethod) => concreteType.Name == "Foo";
        /// </code>
        /// </remarks>
        public static void InterceptNewInstances(this MethodDefinition target, Func<MethodReference, TypeReference, MethodReference, bool> typeFilter,
            Func<MethodReference, bool> methodFilter)
        {
            var redirector = new RedirectNewInstancesToActivator(typeFilter);
            target.InterceptNewInstancesWith(redirector, methodFilter);
        }

        /// <summary>
        /// Modifies a <paramref name="target"/> type to support intercepting calls to the 'new' operator.
        /// </summary>
        /// <param name="target">The item to be modified.</param>
        /// <param name="typeFilter">The functor that determines which type instantiations should be intercepted.</param>
        /// <param name="methodFilter">The filter that determines which host methods will be modified</param>
        /// <remarks>
        /// The type filter determines which concrete types and constructors should be intercepted at runtime.
        /// For example, the following functor code intercepts types named "Foo":
        /// <code>
        ///     Func&lt;MethodReference, TypeReference, MethodReference&gt; filter = 
        ///     (constructor, concreteType, hostMethod) => concreteType.Name == "Foo";
        /// </code>
        /// </remarks>
        public static void InterceptNewInstances(this TypeDefinition target, Func<MethodReference, TypeReference, MethodReference, bool> typeFilter,
            Func<MethodReference, bool> methodFilter)
        {
            var redirector = new RedirectNewInstancesToActivator(typeFilter);
            target.InterceptNewInstancesWith(redirector, methodFilter);
        }

        /// <summary>
        /// Modifies a <paramref name="target"/> module to support intercepting calls to the 'new' operator.
        /// </summary>
        /// <param name="target">The item to be modified.</param>
        /// <param name="typeFilter">The functor that determines which type instantiations should be intercepted.</param>
        /// <param name="methodFilter">The filter that determines which host methods will be modified</param>
        /// <remarks>
        /// The type filter determines which concrete types and constructors should be intercepted at runtime.
        /// For example, the following functor code intercepts types named "Foo":
        /// <code>
        ///     Func&lt;MethodReference, TypeReference, MethodReference&gt; filter = 
        ///     (constructor, concreteType, hostMethod) => concreteType.Name == "Foo";
        /// </code>
        /// </remarks>
        public static void InterceptNewInstances(this ModuleDefinition target, Func<MethodReference, TypeReference, MethodReference, bool> typeFilter,
            Func<MethodReference, bool> methodFilter)
        {
            var redirector = new RedirectNewInstancesToActivator(typeFilter);
            target.InterceptNewInstancesWith(redirector, methodFilter);
        }

        /// <summary>
        /// Modifies the methods in the given <paramref name="target"/> using the custom <see cref="INewObjectWeaver"/> instance.
        /// </summary>
        /// <param name="target">The host that contains the methods that will be modified.</param>
        /// <param name="weaver">The custom <see cref="INewObjectWeaver"/> that will replace all calls to the new operator with the custom code emitted by the given weaver.</param>
        /// <param name="filter">The method filter that will determine which methods should be modified.</param>
        public static void InterceptNewInstancesWith(this AssemblyDefinition target, INewObjectWeaver weaver, Func<MethodReference, bool> filter)
        {
            IReflectionStructureVisitable visitable = target;
            visitable.InterceptNewInstancesWithInternal(weaver, filter);
        }

        /// <summary>
        /// Modifies the methods in the given <paramref name="target"/> using the custom <see cref="INewObjectWeaver"/> instance.
        /// </summary>
        /// <param name="target">The host that contains the methods that will be modified.</param>
        /// <param name="weaver">The custom <see cref="INewObjectWeaver"/> that will replace all calls to the new operator with the custom code emitted by the given weaver.</param>
        /// <param name="filter">The method filter that will determine which methods should be modified.</param>
        public static void InterceptNewInstancesWith(this ModuleDefinition target, INewObjectWeaver weaver, Func<MethodReference, bool> filter)
        {
            IReflectionVisitable visitable = target;
            visitable.InterceptNewInstancesWithInternal(weaver, filter);
        }

        /// <summary>
        /// Modifies the methods in the given <paramref name="target"/> using the custom <see cref="INewObjectWeaver"/> instance.
        /// </summary>
        /// <param name="target">The host that contains the methods that will be modified.</param>
        /// <param name="weaver">The custom <see cref="INewObjectWeaver"/> that will replace all calls to the new operator with the custom code emitted by the given weaver.</param>
        /// <param name="filter">The method filter that will determine which methods should be modified.</param>
        public static void InterceptNewInstancesWith(this TypeDefinition target, INewObjectWeaver weaver, Func<MethodReference, bool> filter)
        {
            IReflectionVisitable visitable = target;
            visitable.InterceptNewInstancesWithInternal(weaver, filter);
        }

        /// <summary>
        /// Modifies the methods in the given <paramref name="target"/> using the custom <see cref="INewObjectWeaver"/> instance.
        /// </summary>
        /// <param name="target">The host that contains the methods that will be modified.</param>
        /// <param name="weaver">The custom <see cref="INewObjectWeaver"/> that will replace all calls to the new operator with the custom code emitted by the given weaver.</param>
        /// <param name="filter">The method filter that will determine which methods should be modified.</param>
        public static void InterceptNewInstancesWith(this MethodDefinition target, INewObjectWeaver weaver, Func<MethodReference, bool> filter)
        {
            IReflectionVisitable visitable = target;
            visitable.InterceptNewInstancesWithInternal(weaver, filter);
        }

        private static void InterceptNewInstancesWithInternal(this IReflectionVisitable target, INewObjectWeaver weaver, Func<MethodReference, bool> filter)
        {
            var interceptNewCalls = new InterceptNewCalls(weaver);
            target.WeaveWith(interceptNewCalls, filter);
        }

        private static void InterceptNewInstancesWithInternal(this IReflectionStructureVisitable target, INewObjectWeaver weaver, Func<MethodReference, bool> filter)
        {
            var interceptNewCalls = new InterceptNewCalls(weaver);
            target.WeaveWith(interceptNewCalls, filter);
        }
    }
}