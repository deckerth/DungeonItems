Imports DungeonItems.Model

Namespace Global.DungeonItems.ViewModels

    Public Class PerkViewModel
        Inherits BindableBase

        Private Model As Perk

        Public Property Modified As Boolean = False

        Public Sub New(model As Perk)
            Me.Model = model
        End Sub

        Public Property IsUnique As Boolean
            Get
                Return Model.IsUnique
            End Get
            Set(value As Boolean)
                If value <> Model.IsUnique Then
                    Model.IsUnique = value
                    OnPropertyChanged("IsUnique")
                    Modified = True
                End If
            End Set
        End Property

        Public Property Description As String
            Get
                Return Model.Description
            End Get
            Set(value As String)
                If Not value.Equals(Model.Description) Then
                    Model.Description = value
                    OnPropertyChanged("Description")
                    Modified = True
                End If
            End Set
        End Property

    End Class

End Namespace
