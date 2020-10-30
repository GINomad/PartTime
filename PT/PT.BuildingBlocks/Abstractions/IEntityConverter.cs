using System;
using System.Collections.Generic;

namespace PT.BuildingBlocks.Abstractions
{
    public interface IEntityConverter
    {
        /// <summary>
        /// Assigns properties of one object to another by registered mapping configuration.
        /// </summary>
        /// <typeparam name="TIn">Type of object to be assigned from.</typeparam>
        /// <typeparam name="TOut">Type of object to be assigned to.</typeparam>
        /// <param name="source">Object to be assigned from.</param>
        /// <param name="destination">Object to be assigned to.</param>
        void AssignTo<TIn, TOut>(TIn source, TOut destination);

        /// <summary>
        /// Converts passed object to the specified type.
        /// </summary>
        /// <typeparam name="TIn">Type of object to be converted from.</typeparam>
        /// <typeparam name="TOut">Type of the output object.</typeparam>
        /// <param name="source">Object to be converted from.</param>
        /// <returns>Converted object.</returns>
        TOut ConvertTo<TIn, TOut>(TIn source);

        /// <summary>
        /// Maps two collections (update existing items, add new and remove unnecessary)
        /// </summary>
        /// <typeparam name="TSource">Type of collection object to be converted from</typeparam>
        /// <typeparam name="TDestination">Type of collection object to be converted from</typeparam>
        /// <param name="newCollection">A collection with changed items</param>
        /// <param name="existingCollection">An existing collection that need to be changed</param>
        /// <param name="mapFunction">Instructions for match collections items</param>
        /// <param name="updateExistingItemFunction">Instructions for map collection items</param>
        /// <param name="deleteDestItemFunction">Instructions for delete exitsting item</param>
        /// <param name="addNewItemFunction">Instructions for add new item to existing collection</param>
        void MapCollections<TSource, TDestination>(IEnumerable<TSource> newCollection, IEnumerable<TDestination> existingCollection, Func<TSource, TDestination, bool> mapFunction, Action<TSource, TDestination> updateExistingItemFunction, Action<TDestination> deleteDestItemFunction, Action<TSource> addNewItemFunction);
    }
}
