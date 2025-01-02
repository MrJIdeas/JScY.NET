using System;
using JScy.NET.Physics.Simulationen.Wavefunctions.Enums;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.AttributesCustom
{
    public class SpaceDirectionAttribute(ESpaceDirection direction) : Attribute
    {
        public readonly ESpaceDirection Direction = direction;
    }
}