using UnityEngine;
using System.Collections;

public class Item
{
	public string name;

	public Item(string _name) {
		name = _name;
	}

	#region IEquatable<Item> Members

    public bool Equals(Item other)
    {
        

        return this.name == other.name;
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as Item);
    }

    #endregion
}