using System;
using UnityEngine;

public interface I_ESpell
{
    void Cast(ESpell eSpell);
}

public abstract class SpellActionBase : MonoBehaviour, I_ESpell
{
    public abstract void Cast(ESpell eSpell);
}