using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Item : Interactable
{
	public string name;
    public bool unique;
    public Item self;
    public bool acumulative;
    public int qtd;

    public Item() {

    }

	public Item(string _name) {
		name = _name;
	}

    void Update() {
        if(actualColider == null || actualColider.gameObject.tag != "Player") return;
        if(CrossPlatformInputManager.GetButton("Submit")) {
            Player.inventory.Add(self);
            gameObject.active = false;
            base.OnTriggerExit2D(actualColider);
            actualColider = null;
            Destroy(gameObject);
        }
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