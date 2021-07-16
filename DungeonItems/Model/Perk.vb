Namespace Global.DungeonItems.Model

    Public Class Perk

        Private _Id As Guid
        Public ReadOnly Property Id As Guid
            Get
                Return _Id
            End Get
        End Property

        Public Property Description As String

        Public Sub New(Id As Guid)
            _Id = Id
        End Sub

        Public Sub New()
            _Id = Guid.NewGuid()
        End Sub

    End Class

End Namespace
