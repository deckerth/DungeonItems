Imports DungeonItems.Model.Item

Namespace Global.DungeonItems.Model

    Public Class ItemFactory

        Public Shared Function CreateItem(Type As ItemType, Id As Guid) As Item
            Select Case Type
                Case ItemType.Armor
                    Return CreateArmor(Id)
                Case ItemType.Melee
                    Return CreateMelee(Id)
                Case ItemType.Artillery
                    Return CreateArtillery(Id)
            End Select
            Return Nothing
        End Function

        Public Shared Function CreateItem(Type As ItemType) As Item
            Select Case Type
                Case ItemType.Armor
                    Return CreateArmor()
                Case ItemType.Melee
                    Return CreateMelee()
                Case ItemType.Artillery
                    Return CreateArtillery()
            End Select
            Return Nothing
        End Function

        Public Shared Function CreateArmor(id As Guid) As Armor
            Return New Armor(id)
        End Function

        Public Shared Function CreateMelee(id As Guid) As Melee
            Return New Melee(id)
        End Function

        Public Shared Function CreateArtillery(id As Guid) As Artillery
            Return New Artillery(id)
        End Function

        Public Shared Function CreateArmor() As Armor
            Return New Armor()
        End Function

        Public Shared Function CreateMelee() As Melee
            Return New Melee()
        End Function

        Public Shared Function CreateArtillery() As Artillery
            Return New Artillery()
        End Function

    End Class

End Namespace
