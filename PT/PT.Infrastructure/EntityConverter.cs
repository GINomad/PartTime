using AutoMapper;
using PT.BuildingBlocks.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PT.Infrastructure
{
    public class EntityConverter : IEntityConverter
    {
        public static Action<IMapperConfigurationExpression> GetDefaultMapperConfiguration(string[] mappingContainerAssemblyNames)
        {
            var assemblies = mappingContainerAssemblyNames.Select(assemblyName => Assembly.Load(assemblyName)).ToArray();
            return GetDefaultMapperConfiguration(assemblies);
        }

        public static Action<IMapperConfigurationExpression> GetDefaultMapperConfiguration(Assembly[] mappingContainerAssemblies)
        {
            return cfg => cfg.AddMaps(mappingContainerAssemblies);
        }

        private readonly IMapper _defaultMapper;
        private readonly MapperConfiguration _defaultCofiguration;

        public EntityConverter(Action<IMapperConfigurationExpression> configExpression, Func<Type, object> serviceCtor, bool skipMappingsValidation = false)
        {
            _defaultCofiguration = new MapperConfiguration(configExpression);
            if (!skipMappingsValidation)
            {
                _defaultCofiguration.AssertConfigurationIsValid();
            }

            _defaultMapper = _defaultCofiguration.CreateMapper(serviceCtor);
        }

        /// <summary>
        /// Assigns properties of one object to another by registered mapping configuration.
        /// </summary>
        /// <typeparam name="TIn">Type of object to be assigned from.</typeparam>
        /// <typeparam name="TOut">Type of object to be assigned to.</typeparam>
        /// <param name="source">Object to be assigned from.</param>
        /// <param name="destination">Object to be assigned to.</param>
        public void AssignTo<TIn, TOut>(TIn source, TOut destination)
        {
            _defaultMapper.Map<TIn, TOut>(source, destination);
        }

        /// <summary>
        /// Converts passed object to the specified type.
        /// </summary>
        /// <typeparam name="TIn">Type of object to be converted from.</typeparam>
        /// <typeparam name="TOut">Type of the output object.</typeparam>
        /// <param name="source">Object to be converted from.</param>
        /// <returns>Converted object.</returns>
        public TOut ConvertTo<TIn, TOut>(TIn source)
        {
            return _defaultMapper.Map<TOut>(source);
        }

        public void MapCollections<TSource, TDestination>(
          IEnumerable<TSource> newCollection,
          IEnumerable<TDestination> existingCollection,
          Func<TSource, TDestination, bool> mapFunction,
          Action<TSource, TDestination> updateExistingItemFunction,
          Action<TDestination> deleteDestItemFunction,
          Action<TSource> addNewItemFunction)
        {
            var newItemsList = newCollection != null ? newCollection.ToList() : new List<TSource>();
            var existingItemsList = existingCollection != null ? existingCollection.ToList() : new List<TDestination>();

            foreach (var existingItem in existingItemsList)
            {
                TSource newItem = newCollection.FirstOrDefault(item => mapFunction(item, existingItem));

                if (newItem != null && !newItem.Equals(default(TSource)))
                {
                    //update
                    updateExistingItemFunction(newItem, existingItem);
                    newItemsList.Remove(newItem);
                }
                else
                {
                    //delete
                    deleteDestItemFunction(existingItem);
                }
            }

            if (newItemsList != null && newItemsList.Any())
            {
                foreach (var newItem in newItemsList)
                {
                    addNewItemFunction(newItem);
                }
            }
        }
    }
}
