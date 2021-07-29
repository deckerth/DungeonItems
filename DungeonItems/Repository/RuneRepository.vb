Imports DungeonItems.Model

Namespace Global.DungeonItems.Repository

    Public Class RuneRepository

        Private Shared _current As RuneRepository
        Public Shared ReadOnly Property Current
            Get
                If _current Is Nothing Then
                    _current = New RuneRepository
                End If
                Return _current
            End Get
        End Property

        Public Property Runes As New ObservableCollection(Of Rune)

        Public Function GetRune(name As String)
            Return Runes.FirstOrDefault(Function(other) other.Name = name)
        End Function

        Public Sub New()
            Runes.Add(New Rune("ms-appx:///Assets/Images/RuneA.png") With {.Name = "Rune A"})
            Runes.Add(New Rune("ms-appx:///Assets/Images/RuneC.png") With {.Name = "Rune C"})
            Runes.Add(New Rune("ms-appx:///Assets/Images/RuneI.png") With {.Name = "Rune I"})
            Runes.Add(New Rune("ms-appx:///Assets/Images/RuneO.png") With {.Name = "Rune O"})
            Runes.Add(New Rune("ms-appx:///Assets/Images/RuneP.png") With {.Name = "Rune P"})
            Runes.Add(New Rune("ms-appx:///Assets/Images/RuneR.png") With {.Name = "Rune R"})
            Runes.Add(New Rune("ms-appx:///Assets/Images/RuneS.png") With {.Name = "Rune S"})
            Runes.Add(New Rune("ms-appx:///Assets/Images/RuneT.png") With {.Name = "Rune T"})
            Runes.Add(New Rune("ms-appx:///Assets/Images/RuneU.png") With {.Name = "Rune U"})
        End Sub
    End Class

End Namespace
