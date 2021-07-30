Imports DungeonItems.Model
Imports DungeonItems.ViewModels

Namespace Global.DungeonItems.Views

    Public NotInheritable Class ArmorPage
        Inherits Page

        Public Property ViewModel As ItemViewModel

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            ViewModel = DirectCast(e.Parameter, ItemViewModel)
            ViewModel.DisplayImage = ItemImageDisplay
            ViewModel.EditImage = ItemImageEdit
            Await ViewModel.LoadImageAsync()
        End Sub


        Private Sub PerkItem_Click(sender As Object, e As RoutedEventArgs)
            Dim perkButton = DirectCast(sender, Button)
            Dim perk = DirectCast(perkButton.DataContext, Perk)
            ViewModel.AddPerk(perk)
        End Sub

        Private Sub AddPerkFlyout_Opening(sender As Object, e As Object) Handles AddPerkFlyout.Opening
            ViewModel.AllPerks.Refresh()
        End Sub

        Private Sub EnchantmentItem_Click(sender As Object, e As RoutedEventArgs)
            Dim enchantmentButton = DirectCast(sender, Button)
            Dim enchantment = DirectCast(enchantmentButton.DataContext, Enchantment)
            ViewModel.AddEnchantment(enchantment)
        End Sub

        Private Sub RuneItem_Click(sender As Object, e As RoutedEventArgs)
            Dim runeButton = DirectCast(sender, Button)
            Dim rune = DirectCast(runeButton.DataContext, Rune)
            ViewModel.AddRune(rune)
        End Sub

        Private Sub AddEnchantmentFlyout_Opening(sender As Object, e As Object) Handles AddEnchantmentFlyout.Opening
            ViewModel.AllEnchantments.Refresh()
        End Sub
    End Class

End Namespace
