using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class BoolTable : MonoBehaviour
{
    public bool Value {get{return m_finalValue;} set{UpdateBoolTable(value);}}

    bool m_finalValue;
    List<bool> m_ValuesTable;

    void UpdateBoolTable(bool value)
    {
        UpdateTable(value);
    }

    void UpdateTable(bool value)
    {
        for(int i=0; i<m_ValuesTable.Count; i++)
        {
            if(m_ValuesTable[i] == !value)
            {
                m_ValuesTable.RemoveAt(i);
                break;
            }

            if(i+1 == m_ValuesTable.Count)
            {        
                m_ValuesTable.Add(value);
            }
        }

        if(m_ValuesTable.Count == 0)
        {
            m_finalValue = value;
        }
    }

    public static bool operator==(BoolTable a, bool b)
    {
        return a.Value == b;
    }

    public static bool operator!=(BoolTable a, bool b)
    {
        return a.Value != b;
    }
}
