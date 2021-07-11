Imports DungeonItems.Model

Namespace Global.DungeonItems.ViewModels

    Public Class ArtilleryViewModel
        Inherits ItemViewModel

        Public Property Force As Double
            Get
                Return DirectCast(Model, Artillery).Force
            End Get
            Set(value As Double)
                If value <> DirectCast(Model, Artillery).Force Then
                    DirectCast(Model, Artillery).Force = value
                    OnPropertyChanged("Force")
                    Modified = True
                End If
            End Set
        End Property

        Public Property Speed As Double
            Get
                Return DirectCast(Model, Artillery).Speed
            End Get
            Set(value As Double)
                If value <> DirectCast(Model, Artillery).Speed Then
                    DirectCast(Model, Artillery).Speed = value
                    OnPropertyChanged("Speed")
                    Modified = True
                End If
            End Set
        End Property

        Public Property Ammo As Double
            Get
                Return DirectCast(Model, Artillery).Ammo
            End Get
            Set(value As Double)
                If value <> DirectCast(Model, Artillery).Ammo Then
                    DirectCast(Model, Artillery).Ammo = value
                    OnPropertyChanged("Ammo")
                    Modified = True
                End If
            End Set
        End Property

        Public Sub New(model As Item)
            MyBase.New(model)
        End Sub

    End Class

End Namespace
