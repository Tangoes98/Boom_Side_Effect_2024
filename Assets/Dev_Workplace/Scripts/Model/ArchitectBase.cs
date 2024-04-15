using System.Collections.Generic;
using System.Linq;

public class ArchitectBase
{
    public enum ArchitectType {
        DEFENCE_TOWER,
        BARRACK
    }

    public string Code {get;}
    public string Name {get;}
    public ArchitectType Type {get;}
    public bool IsMutant {get;} 

    public bool IsUpgradable(int curlevel) => _properties.Keys.Max()>curlevel;



    private Dictionary<int, ArchitectProperty> _properties;

    public ArchitectBase(string code, string name, ArchitectType type, bool isMutant, 
        Dictionary<int, ArchitectProperty> properties) {

            if(!isMutant && properties.Values.Any(p => p.Modifer(0)!=null)) {
                throw new System.Exception("Only mutant architect can have modifer in them");
            }
            if(properties.Values.Any(p=>p.Type != Type)) {
                throw new System.Exception("architect type doesn't match property type");
            }
            
            
            Code=code;
            Name=name;
            Type=type;
            IsMutant=isMutant;
            _properties=properties;
    }

    public ArchitectProperty GetProperty(int level)  => _properties[level];

}
