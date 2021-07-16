Namespace Global.DungeonItems.Views

    Public NotInheritable Class EmptyPage
        Inherits Page

        Public Property InfoText = ""

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            InfoText = DirectCast(e.Parameter, String)
        End Sub


    End Class

End Namespace
