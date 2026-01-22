using System.ComponentModel;
using Discord;

namespace MareJira.Objects;

public enum Priority {
    [Description("<:lowest:1461361833125351578>")]
    Lowest = 1,
    [Description("<:low:1461361848040292374>")]
    Low = 2,
    [Description("<:medium:1461361863089459344>")]
    Medium = 3,
    [Description("<:high:1461361879686316083>")]
    High = 4,
    [Description("<:highest:1461361893678645349>")]
    Highest = 5
}

public static class EnumExtensionMethods {  
    public static string GetEnumDescription(this Enum enumValue) {  
        
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());  
        var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);  
  
        return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();  
    }
    
    public static Color GetEnumColors(this Enum enumValue) {
        
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString()); 
        
        switch (fieldInfo.Name) {
            case "Lowest":
                return new (0x416dff);
            case "Low":
                return new (0x3daaff);
            case "Medium":
                return new (0xffbb3d);
            case "High":
                return new (0xff8953);
            case "Highest":
                return new (0xff5454);
            default:
                return new(0xffffff);
        }
    }
}  