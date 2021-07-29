Imports DungeonItems.Model
Imports DungeonItems.Repository

Namespace Global.DungeonItems.ViewModels

    Public Class PerkViewModel
        Inherits BindableBase

        Public Property Model As Perk

        Public Property Modified As Boolean = False

        Public Sub New(model As Perk)
            Me.Model = model
        End Sub

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

        Friend Sub Save(repository As PerkRepository)
            If Modified Then
                repository.UpdatePerk(Model)
                Modified = False
            End If
        End Sub

        Friend Sub Delete(repository As PerkRepository)
            repository.DeletePerk(Model)
            Modified = False
        End Sub
    End Class

End Namespace
