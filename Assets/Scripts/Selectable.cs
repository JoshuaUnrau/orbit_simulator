using System.Collections.Generic;
using UnityEngine;
 
public class Selectable : MonoBehaviour
{
 
    internal bool isSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            _isSelected = value;
            //Replace this with your custom code. What do you want to happen to a Selectable when it get's (de)selected?
            //Renderer r = GetComponentInChildren<Renderer>();
            var r = GetComponentsInChildren<Renderer>();
            if (r != null)
            {
                foreach (var element in r)
                {
                    element.material.color = value ? Color.red : Color.white;    
                }
            }
        }
    }
 
    private bool _isSelected;
 
    void OnEnable()
    {
        Selection.selectables.Add(this);
    }
 
    void OnDisable()
    {
        Selection.selectables.Remove(this);
    }
 
}