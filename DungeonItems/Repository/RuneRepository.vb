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

        Public Property Runes As ObservableCollection(Of Rune)

        Public Function GetRune(name As String)
            Return Runes.FirstOrDefault(Function(other) other.Name = name)
        End Function

        Public Sub New()
            Runes.Add(New Rune With {.Name = "Creeper Woods"})
            Runes.Add(New Rune With {.Name = "Cacti Canyon"})
            Runes.Add(New Rune With {.Name = "Soggy Swamp"})
            Runes.Add(New Rune With {.Name = "Pumpkin Pastures"})
            Runes.Add(New Rune With {.Name = "Redstone Mines"})
            Runes.Add(New Rune With {.Name = "Fiery Forge"})
            Runes.Add(New Rune With {.Name = "Desert Temple"})
            Runes.Add(New Rune With {.Name = "Highblock Hall"})
            Runes.Add(New Rune With {.Name = "Obsidian Pinnacle"})
        End Sub
    End Class

End Namespace
