Imports DungeonItems.ViewModels

Namespace Global.DungeonItems.Views

    Public NotInheritable Class EnchantmentsEditorPage
        Inherits Page

        Public Property ViewModel As EnchantmentsViewModel

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            ViewModel = DirectCast(e.Parameter, EnchantmentsViewModel)
            GroupedEnchantmentsCVS.Source = ViewModel.GroupedEnhancements.GroupedElements
            ItemsList.ItemsSource = GroupedEnchantmentsCVS.View
            ViewModel.DetailFrame = DetailFrame
            ViewModel.RootFrame = Frame
            ViewModel.Navigate()
        End Sub

        Private Sub DoNotDeleteButton_Click(sender As Object, e As RoutedEventArgs)
            DeleteItemFlyout.Hide()
        End Sub

        Private Sub DoDeleteButton_Click(sender As Object, e As RoutedEventArgs)
            DeleteItemFlyout.Hide()
        End Sub

    End Class

End Namespace
