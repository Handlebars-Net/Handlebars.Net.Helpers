using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Stef.Validation;
#if SIMPLE_JSON_DYNAMIC
using System.Dynamic;
#endif

namespace JsonConverter.SimpleJson;

/// <summary>
/// Represents the json object.
/// </summary>
[GeneratedCode("simple-json", "1.0.0")]
[EditorBrowsable(EditorBrowsableState.Never)]
#if SIMPLE_JSON_OBJARRAYINTERNAL
internal
#else
public
#endif
    class JsonObject :
#if SIMPLE_JSON_DYNAMIC
    DynamicObject,
#endif
    IDictionary<string, object>
{
    /// <summary>
    /// The internal member dictionary.
    /// </summary>
    private readonly Dictionary<string, object> _members;

    /// <summary>
    /// Initializes a new instance of <see cref="JsonObject"/>.
    /// </summary>
    public JsonObject()
    {
        _members = new Dictionary<string, object>();
    }

    /// <summary>
    /// Initializes a new instance of <see cref="JsonObject"/>.
    /// </summary>
    /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1"/> for the type of the key.</param>
    public JsonObject(IEqualityComparer<string> comparer)
    {
        _members = new Dictionary<string, object>(comparer);
    }

    /// <summary>
    /// Gets the <see cref="System.Object"/> at the specified index.
    /// </summary>
    /// <value></value>
    public object? this[int index] => GetAtIndex(_members, index);

    internal static object? GetAtIndex(IDictionary<string, object> obj, int index)
    {
        Guard.NotNull(obj);
        Guard.Condition(index, idx => idx >= obj.Count);

        int i = 0;
        foreach (KeyValuePair<string, object> o in obj)
        {
            if (i++ == index)
            {
                return o.Value;
            }
        }

        return null;
    }

    /// <summary>
    /// Adds the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void Add(string key, object value)
    {
        _members.Add(key, value);
    }

    /// <summary>
    /// Determines whether the specified key contains key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>
    ///     <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
    /// </returns>
    public bool ContainsKey(string key)
    {
        return _members.ContainsKey(key);
    }

    /// <summary>
    /// Gets the keys.
    /// </summary>
    /// <value>The keys.</value>
    public ICollection<string> Keys => _members.Keys;

    /// <summary>
    /// Removes the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public bool Remove(string key)
    {
        return _members.Remove(key);
    }

    /// <summary>
    /// Tries the get value.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public bool TryGetValue(string key, out object value)
    {
        return _members.TryGetValue(key, out value);
    }

    /// <summary>
    /// Gets the values.
    /// </summary>
    /// <value>The values.</value>
    public ICollection<object> Values => _members.Values;

    /// <summary>
    /// Gets or sets the <see cref="System.Object"/> with the specified key.
    /// </summary>
    /// <value></value>
    public object this[string key]
    {
        get => _members[key];
        set => _members[key] = value;
    }

    /// <summary>
    /// Adds the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    public void Add(KeyValuePair<string, object> item)
    {
        _members.Add(item.Key, item.Value);
    }

    /// <summary>
    /// Clears this instance.
    /// </summary>
    public void Clear()
    {
        _members.Clear();
    }

    /// <summary>
    /// Determines whether [contains] [the specified item].
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>
    /// 	<c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(KeyValuePair<string, object> item)
    {
        return _members.ContainsKey(item.Key) && _members[item.Key] == item.Value;
    }

    /// <summary>
    /// Copies to.
    /// </summary>
    /// <param name="array">The array.</param>
    /// <param name="arrayIndex">Index of the array.</param>
    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
        Guard.NotNull(array);

        int num = Count;
        foreach (KeyValuePair<string, object> kvp in this)
        {
            array[arrayIndex++] = kvp;
            if (--num <= 0)
            {
                return;
            }
        }
    }

    /// <summary>
    /// Gets the count.
    /// </summary>
    /// <value>The count.</value>
    public int Count => _members.Count;

    /// <summary>
    /// Gets a value indicating whether this instance is read only.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
    /// </value>
    public bool IsReadOnly => false;

    /// <summary>
    /// Removes the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns></returns>
    public bool Remove(KeyValuePair<string, object> item)
    {
        return _members.Remove(item.Key);
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns></returns>
    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        return _members.GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return _members.GetEnumerator();
    }

    /// <summary>
    /// Returns a json <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
    /// </summary>
    /// <returns>
    /// A json <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
    /// </returns>
    public override string ToString()
    {
        return SimpleJson.SerializeObject(this) ?? string.Empty;
    }

#if SIMPLE_JSON_DYNAMIC
    /// <summary>
    /// Provides implementation for type conversion operations. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations that convert an object from one type to another.
    /// </summary>
    /// <param name="binder">Provides information about the conversion operation. The binder.Type property provides the type to which the object must be converted. For example, for the statement (String)sampleObject in C# (CType(sampleObject, Type) in Visual Basic), where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Type returns the <see cref="T:System.String"/> type. The binder.Explicit property provides information about the kind of conversion that occurs. It returns true for explicit conversion and false for implicit conversion.</param>
    /// <param name="result">The result of the type conversion operation.</param>
    /// <returns>
    /// Always returns true.
    /// </returns>
    public override bool TryConvert(ConvertBinder binder, out object result)
    {
        // <pex>
        if (binder == null)
            throw new ArgumentNullException("binder");
        // </pex>
        Type targetType = binder.Type;

        if ((targetType == typeof(IEnumerable)) ||
            (targetType == typeof(IEnumerable<KeyValuePair<string, object>>)) ||
            (targetType == typeof(IDictionary<string, object>)) ||
            (targetType == typeof(IDictionary)))
        {
            result = this;
            return true;
        }

        return base.TryConvert(binder, out result);
    }

    /// <summary>
    /// Provides the implementation for operations that delete an object member. This method is not intended for use in C# or Visual Basic.
    /// </summary>
    /// <param name="binder">Provides information about the deletion.</param>
    /// <returns>
    /// Always returns true.
    /// </returns>
    public override bool TryDeleteMember(DeleteMemberBinder binder)
    {
        // <pex>
        if (binder == null)
            throw new ArgumentNullException("binder");
        // </pex>
        return _members.Remove(binder.Name);
    }

    /// <summary>
    /// Provides the implementation for operations that get a value by index. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for indexing operations.
    /// </summary>
    /// <param name="binder">Provides information about the operation.</param>
    /// <param name="indexes">The indexes that are used in the operation. For example, for the sampleObject[3] operation in C# (sampleObject(3) in Visual Basic), where sampleObject is derived from the DynamicObject class, <paramref name="indexes"/> is equal to 3.</param>
    /// <param name="result">The result of the index operation.</param>
    /// <returns>
    /// Always returns true.
    /// </returns>
    public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object? result)
    {
        Guard.NotNull(binder);
        Guard.NotNull(indexes);

        if (indexes.Length == 1)
        {
            result = ((IDictionary<string, object>)this)[(string)indexes[0]];
            return true;
        }

        result = null;
        return true;
    }

    /// <summary>
    /// Provides the implementation for operations that get member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations such as getting a value for a property.
    /// </summary>
    /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the Console.WriteLine(sampleObject.SampleProperty) statement, where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
    /// <param name="result">The result of the get operation. For example, if the method is called for a property, you can assign the property value to <paramref name="result"/>.</param>
    /// <returns>
    /// Always returns true.
    /// </returns>
    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        if (_members.TryGetValue(binder.Name, out var value))
        {
            result = value;
            return true;
        }

        result = null;
        return true;
    }

    /// <summary>
    /// Provides the implementation for operations that set a value by index. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations that access objects by a specified index.
    /// </summary>
    /// <param name="binder">Provides information about the operation.</param>
    /// <param name="indexes">The indexes that are used in the operation. For example, for the sampleObject[3] = 10 operation in C# (sampleObject(3) = 10 in Visual Basic), where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, <paramref name="indexes"/> is equal to 3.</param>
    /// <param name="value">The value to set to the object that has the specified index. For example, for the sampleObject[3] = 10 operation in C# (sampleObject(3) = 10 in Visual Basic), where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, <paramref name="value"/> is equal to 10.</param>
    /// <returns>
    /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.
    /// </returns>
    public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
    {
        if (indexes == null) throw new ArgumentNullException("indexes");
        if (indexes.Length == 1)
        {
            ((IDictionary<string, object>)this)[(string)indexes[0]] = value;
            return true;
        }
        return base.TrySetIndex(binder, indexes, value);
    }

    /// <summary>
    /// Provides the implementation for operations that set member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations such as setting a value for a property.
    /// </summary>
    /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member to which the value is being assigned. For example, for the statement sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
    /// <param name="value">The value to set to the member. For example, for sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, the <paramref name="value"/> is "Test".</param>
    /// <returns>
    /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)
    /// </returns>
    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        // <pex>
        if (binder == null)
            throw new ArgumentNullException("binder");
        // </pex>
        _members[binder.Name] = value;
        return true;
    }

    /// <summary>
    /// Returns the enumeration of all dynamic member names.
    /// </summary>
    /// <returns>
    /// A sequence that contains dynamic member names.
    /// </returns>
    public override IEnumerable<string> GetDynamicMemberNames()
    {
        foreach (var key in Keys)
            yield return key;
    }
#endif
}