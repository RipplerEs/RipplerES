using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RipplerES.CommandHandler
{
    public static class AggregateTypeExtensions
    {
        public static string GetFriendlName(this Type type)
        {
            return type.GetTypeInfo().GetCustomAttribute<FriendlyName>()?.Name;
        }
        public static ConstructorInfo GetConstructor(this Type type)
        {
            return type.GetTypeInfo().GetConstructors().Single(c => !c.GetParameters().Any());
        }

        public static IDictionary<Type, MethodInfo> GetAggregateEvents(this Type type)
        {
            var aggregateEvents = type.GetRuntimeMethods().Where(m => m.IsValidAggregateEventMethod());
            return aggregateEvents.ToDictionary(e => e.GetParameterType(), e => e);

        }

        public static IDictionary<Type, MethodInfo> GetAggregateCommands(this Type type)
        {
            var aggregateEvents = type.GetRuntimeMethods().Where(m => m.IsValidAggregateCommandMethod());
            return aggregateEvents.ToDictionary(e => e.GetParameterType(), e => e);

        }

        public static bool IsValidAggregateEventMethod(this MethodInfo methodInfo)
        {
            return methodInfo.HasVoidReturnType()
                   && methodInfo.HasOneParameter()
                   && methodInfo.ParameterIsAggregateEvent();
        }

        public static bool IsValidAggregateCommandMethod(this MethodInfo methodInfo)
        {
            var method = methodInfo.HasOneParameter();

            return methodInfo.HasAggregateCommandResult()
                   && methodInfo.HasOneParameter()
                   && methodInfo.ParameterIsAggregateCommand();
        }

        public static Type GetParameterType(this MethodInfo methodInfo)
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
            if (!methodInfo.ReturnType.GetTypeInfo().IsGenericType) return false;
            return methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(IAggregateCommandResult<>);
        }

        private static bool IsIAggregateEvent(this Type type)
        {
            if (!type.GetTypeInfo().IsGenericType) return false;
            return type.GetGenericTypeDefinition() == typeof (IAggregateEvent<>);
        }

        private static bool IsIAggregateCommand(this Type type)
        {
            return type.GetGenericTypeDefinition() == typeof(IAggregateCommand<>);
        }

        private static bool HasGenericParameterOf(this Type type, Type genericParameterType)
        {
            return type.GetTypeInfo().GenericTypeArguments.First() == genericParameterType;
        }
    }
}