Namespace Global.DungeonItems.ValueConverters

    Public Class NotNullToBooleanConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
            If targetType.Equals(GetType(Boolean)) Then
                Dim result As Boolean = value IsNot Nothing
                If parameter IsNot Nothing Then
                    Dim invert As Boolean = System.Convert.ToBoolean(parameter)
                    If invert Then
                        result = Not result
                    End If
                End If
                Return result
            Else
                Throw New ArgumentException("Unsuported type {0}", targetType.FullName)
            End If
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
    End Class

End Namespace
