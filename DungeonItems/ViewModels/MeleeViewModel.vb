Imports DungeonItems.Model

Namespace Global.DungeonItems.ViewModels

    Public Class MeleeViewModel
        Inherits ItemViewModel

        Public Property Force As Double
            Get
                Return DirectCast(Model, Melee).Force
            End Get
            Set(value As Double)
                If value <> DirectCast(Model, Melee).Force Then
                    DirectCast(Model, Melee).Force = value
                    OnPropertyChanged("Force")
                    Modified = True
                End If
            End Set
        End Property

        Public Property Speed As Double
            Get
                Return DirectCast(Model, Melee).Speed
            End Get
            Set(value As Double)
                If value <> DirectCast(Model, Melee).Speed Then
                    DirectCast(Model, Melee).Speed = value
                    OnPropertyChanged("Speed")
                    Modified = True
                End If
            End Set
        End Property

        Public Property Range As Double
            Get
                Return DirectCast(Model, Melee).Range
            End Get
            Set(value As Double)
                If value <> DirectCast(Model, Melee).Range Then
                    DirectCast(Model, Melee).Range = value
                    OnPropertyChanged("Range")
                    Modified = True
                End If
            End Set
        End Property

        Public Sub New(model As Item)
            MyBase.New(model)
        End Sub

    End Class

End Namespace
