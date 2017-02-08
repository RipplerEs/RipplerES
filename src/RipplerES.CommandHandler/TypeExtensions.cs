using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RipplerES.CommandHandler
{
    public static class AggregateTypeExtensions
    {
        public static IDictionary<Type, MethodInfo> GetAggregateEvents(this Type type)
        {
            var aggregateEvents = type.GetRuntimeMethods().Where(m => m.IsValidAggregateEventMethod());
            return aggregateEvents.ToDictionary(e => e.GetParameterType(), e => e);
        }

        public static IDictionary<Type, MethodInfo> GetAggregateCommands(this Type type)
        {
            var aggregateCommands = type.GetRuntimeMethods().Where(m => m.IsValidAggregateCommandMethod());
            return aggregateCommands.ToDictionary(e => e.GetParameterType(), e => e);

        }

        private static bool IsValidAggregateEventMethod(this MethodInfo methodInfo)
        {
            return methodInfo.HasVoidReturnType()
                   && methodInfo.HasOneParameter()
                   && methodInfo.ParameterIsAggregateEvent();
        }

        private static bool IsValidAggregateCommandMethod(this MethodInfo methodInfo)
        {
            return methodInfo.HasAggregateCommandResult()
                   && methodInfo.HasOneParameter()
                   && methodInfo.ParameterIsAggregateCommand();
        }

        private static Type GetParameterType(this MethodInfo methodInfo)
        {
            return methodInfo.GetParameters()
                             .Single()
                             .ParameterType;
        }


        private static bool HasVoidReturnType(this MethodInfo methodInfo)
        {
            return methodInfo.ReturnType == typeof (void);
        }

        private static bool HasOneParameter(this MethodInfo methodInfo)
        {
            return methodInfo.GetParameters().Count() == 1;
        }

        private static bool ParameterIsAggregateEvent(this MethodInfo methodInfo)
        {
            var parameterType = methodInfo.GetParameters()
                                          .Single().ParameterType;

            var interfaces = parameterType.GetTypeInfo().GetInterfaces();
            return interfaces.Any(i => i.IsIAggregateEventInterfaceFor(methodInfo.DeclaringType));
        }

        private static bool ParameterIsAggregateCommand(this MethodInfo methodInfo)
        {
            var parameterType = methodInfo.GetParameters()
                                          .Single().ParameterType;

            var interfaces = parameterType.GetTypeInfo().GetInterfaces();
            return interfaces.Any(i => i.IsIAggregateCommandInterfaceFor(methodInfo.DeclaringType));
        }

        private static bool IsIAggregateEventInterfaceFor(this Type type, Type genericParameterType)
        {
            return type.IsInterface() &&
                   type.IsIAggregateEvent() && 
                   type.HasGenericParameterOf(genericParameterType);
        }

        private static bool IsIAggregateCommandInterfaceFor(this Type type, Type genericParameterType)
        {
            return type.IsInterface() &&
                   type.IsIAggregateCommand() &&
                   type.HasGenericParameterOf(genericParameterType);
        }

        private static bool IsInterface(this Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }

        private static bool HasAggregateCommandResult(this MethodInfo methodInfo)
        {
            var returnType = methodInfo.ReturnType;
            if (!returnType.IsInterface())
            {
                
            }
            var interfaces = returnType.GetTypeInfo().GetInterfaces();

            return interfaces.Any(i => i.IsIAggregateCommandResultInterfaceFor(methodInfo.DeclaringType));
        }

        private static bool IsIAggregateCommandResultInterfaceFor(this Type type, Type genericParameterType)
        {
            return type.IsInterface() &&
                   type.IsIAggregateCommandResult() &&
                   type.HasGenericParameterOf(genericParameterType);
        }

        private static bool IsIAggregateEvent(this Type type)
        {
            if (!type.GetTypeInfo().IsGenericType) return false;
            return type.GetGenericTypeDefinition() == typeof (IAggregateEvent<>);
        }

        private static bool IsIAggregateCommand(this Type type)
        {
            if (!type.GetTypeInfo().IsGenericType) return false;
            return type.GetGenericTypeDefinition() == typeof(IAggregateCommand<>);
        }

        private static bool IsIAggregateCommandResult(this Type type)
        {
            if (!type.GetTypeInfo().IsGenericType) return false;
            return type.GetGenericTypeDefinition() == typeof(IAggregateCommandResult<>);
        }

        private static bool HasGenericParameterOf(this Type type, Type genericParameterType)
        {
            return type.GetTypeInfo().GenericTypeArguments.First() == genericParameterType;
        }
    }
}