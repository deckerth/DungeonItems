Imports DungeonItems.Model
Imports DungeonItems.Model.Item

Namespace Global.DungeonItems.ValueConverters

    Public Class ItemTypeToStringConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
            If value IsNot Nothing Then
                Dim type As ItemType = DirectCast(value, ItemType)
                Return Item.GetStringFromType(type)
            End If
            Return ""
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
            If value IsNot Nothing Then
                Dim typeStr As String = DirectCast(value, String)
                Return Item.GetTypeFromString(typeStr)
            End If
            Return ""
        End Function

    End Class

End Namespace

