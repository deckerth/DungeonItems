Imports OfficeOpenXml
Imports DungeonItems.Model.Item
Imports DungeonItems.Model

Namespace Global.DungeonItems.Repository

    Public Class ImportExportService

        Private Const ItemsWorkbook As String = "Items"
        Private Const PerkRefsWorkbook As String = "PerkRefs"
        Private Const EnchantmentRefsWorkbook As String = "EnchantmentRefs"
        Private Const RunesWorkbook As String = "Runes"
        Private Const PerksWorkbook As String = "Perks"
        Private Const EnchantmentsWorkbook As String = "Enchantments"

        Public Enum ImportOptions
            Add
            Replace
        End Enum

        Private Class Reference
            Public Property ItemId As Guid
            Public Property Reference As Guid
        End Class

        Private Class RuneReference
            Public Property ItemId As Guid
            Public Property Rune As String
        End Class

        Private PerkReferences As New List(Of Reference)
        Private EnchantmentReferences As New List(Of Reference)
        Private RuneReferences As New List(Of RuneReference)

        Public Function Import(InputStream As Stream, ImportOption As ImportOptions) As UpdateCounters
            Dim counters As New UpdateCounters

            If InputStream IsNot Nothing Then
                Using package = New ExcelPackage(InputStream)
                    ImportPerks(package.Workbook.Worksheets(PerksWorkbook))
                    ImportEnchantments(package.Workbook.Worksheets(EnchantmentsWorkbook))
                    ImportReferences(package.Workbook.Worksheets(PerkRefsWorkbook), PerkReferences)
                    ImportReferences(package.Workbook.Worksheets(EnchantmentRefsWorkbook), EnchantmentReferences)
                    ImportRuneReferences(package.Workbook.Worksheets(PerkRefsWorkbook))
                    counters = ImportItems(package.Workbook.Worksheets(ItemsWorkbook), ImportOption)
                End Using
            End If
            Return counters
        End Function

        Private Sub ImportReferences(worksheet As ExcelWorksheet, storage As List(Of Reference))
            Dim rows = worksheet.Dimension.Rows
            For i = 2 To rows

                Dim current As New Reference
                Try
                    current.ItemId = New Guid(DirectCast(worksheet.Cells(i, 1).Value, String))
                Catch ex As Exception
                End Try
                Try
                    current.Reference = New Guid(DirectCast(worksheet.Cells(i, 2).Value, String))
                Catch ex As Exception
                End Try
                storage.Add(current)
            Next
        End Sub

        Private Sub ImportRuneReferences(worksheet As ExcelWorksheet)
            Dim rows = worksheet.Dimension.Rows
            For i = 2 To rows

                Dim current As New RuneReference
                Try
                    current.ItemId = New Guid(DirectCast(worksheet.Cells(i, 1).Value, String))
                Catch ex As Exception
                End Try
                Try
                    current.Rune = worksheet.Cells(i, 2).Value
                Catch ex As Exception
                End Try
                RuneReferences.Add(current)
            Next
        End Sub

        Private Sub ImportPerks(worksheet As ExcelWorksheet)
            Dim rows = worksheet.Dimension.Rows
            For i = 2 To rows

                Dim current As Perk
                Try
                    Dim id As New Guid(DirectCast(worksheet.Cells(i, 1).Value, String))
                    current = New Perk(id)
                Catch ex As Exception
                End Try

#Disable Warning BC42104 ' Die Variable wurde verwendet, bevor ihr ein Wert zugewiesen wurde.
                If current IsNot Nothing Then
#Enable Warning BC42104 ' Die Variable wurde verwendet, bevor ihr ein Wert zugewiesen wurde.
                    Try
                        current.Description = worksheet.Cells(i, 2).Value
                    Catch ex As Exception
                    End Try
                    PerkRepository.Current.Upsert(current)
                End If
            Next
        End Sub

        Private Sub ImportEnchantments(worksheet As ExcelWorksheet)
            Dim rows = worksheet.Dimension.Rows
            For i = 2 To rows
                Dim current As Enchantment
                Try
                    Dim id As Guid = New Guid(DirectCast(worksheet.Cells(i, 1).Value, String))
                    Dim type = Item.GetTypeFromString(worksheet.Cells(i, 2).Value)
                    current = New Enchantment(type, id)
                Catch ex As Exception
                End Try

#Disable Warning BC42104 ' Die Variable wurde verwendet, bevor ihr ein Wert zugewiesen wurde.
                If current IsNot Nothing Then
#Enable Warning BC42104 ' Die Variable wurde verwendet, bevor ihr ein Wert zugewiesen wurde.
                    Try
                        current.Name = worksheet.Cells(1, 3).Value
                    Catch ex As Exception
                    End Try
                    Try
                        current.Description = worksheet.Cells(1, 4).Value
                    Catch ex As Exception
                    End Try
                    Try
                        current.Image = worksheet.Cells(1, 5).Value
                    Catch ex As Exception
                    End Try
                    Try
                        current.mruToken = worksheet.Cells(1, 6).Value
                    Catch ex As Exception
                    End Try
                    EnchantmentRepository.Current.Upsert(current)
                End If
            Next
        End Sub

        Private Function ImportItems(worksheet As ExcelWorksheet, importOption As ImportOptions) As UpdateCounters
            Dim rows = worksheet.Dimension.Rows
            Dim counters As New UpdateCounters
            If importOption = ImportOptions.Replace Then
                ItemRepository.Current.Clear()
            End If
            For i = 2 To rows
                Dim current As Item
                Try
                    Dim id As Guid = New Guid(DirectCast(worksheet.Cells(i, 1).Value, String))
                    Dim type = Item.GetTypeFromString(worksheet.Cells(i, 2).Value)
                    current = ItemFactory.CreateItem(type, id)
                Catch ex As Exception
                End Try

#Disable Warning BC42104 ' Die Variable wurde verwendet, bevor ihr ein Wert zugewiesen wurde.
                If current IsNot Nothing Then
#Enable Warning BC42104 ' Die Variable wurde verwendet, bevor ihr ein Wert zugewiesen wurde.
                    Try
                        current.Name = worksheet.Cells(i, 3).Value
                    Catch ex As Exception
                    End Try
                    Try
                        current.Description = worksheet.Cells(i, 4).Value
                    Catch ex As Exception
                    End Try
                    Try
                        current.Image = worksheet.Cells(i, 5).Value
                    Catch ex As Exception
                    End Try
                    Try
                        current.mruToken = worksheet.Cells(i, 6).Value
                    Catch ex As Exception
                    End Try
                    Try
                        current.IsUnique = worksheet.Cells(i, 7).Value
                    Catch ex As Exception
                    End Try
                    Select Case current.Type
                        Case ItemType.Artillery
                            Dim artillery = DirectCast(current, Artillery)
                            Try
                                artillery.Force = worksheet.Cells(i, 8).Value
                            Catch ex As Exception
                            End Try
                            Try
                                artillery.Speed = worksheet.Cells(i, 9).Value
                            Catch ex As Exception
                            End Try
                            Try
                                artillery.Ammo = worksheet.Cells(i, 10).Value
                            Catch ex As Exception
                            End Try
                        Case ItemType.Melee
                            Dim melee = DirectCast(current, Melee)
                            Try
                                melee.Force = worksheet.Cells(i, 8).Value
                            Catch ex As Exception
                            End Try
                            Try
                                melee.Speed = worksheet.Cells(i, 9).Value
                            Catch ex As Exception
                            End Try
                            Try
                                melee.Range = worksheet.Cells(i, 11).Value
                            Catch ex As Exception
                            End Try
                    End Select

                    For Each r In PerkReferences
                        If r.ItemId = current.Id Then
                            Dim perk = PerkRepository.Current.GetPerk(r.Reference)
                            If perk IsNot Nothing Then
                                current.Perks.Add(perk)
                            End If
                        End If
                    Next
                    For Each r In EnchantmentReferences
                        If r.ItemId = current.Id Then
                            Dim enchantment = EnchantmentRepository.Current.GetEnchantment(r.Reference)
                            If enchantment IsNot Nothing Then
                                current.Enchantments.Add(enchantment)
                            End If
                        End If
                    Next
                    For Each r In RuneReferences
                        If r.ItemId = current.Id Then
                            Dim rune = RuneRepository.Current.GetRune(r.Rune)
                            If rune IsNot Nothing Then
                                current.Runes.Add(rune)
                            End If
                        End If
                    Next

                    counters.Increment(ItemRepository.Current.Upsert(current))
                End If
            Next
            Return counters
        End Function

        Public Sub Export(OutputStream As Stream)
            If OutputStream IsNot Nothing Then
                Dim package = New ExcelPackage(OutputStream)
                While package.Workbook.Worksheets.Count > 0
                    package.Workbook.Worksheets.Delete(0)
                End While
                ExportItems(package)
                ExportPerkReferences(package)
                ExportEnchantmentReferences(package)
                ExportRunes(package)
                ExportPerks(package)
                ExportEnchantments(package)
                package.Save()
            End If

        End Sub

        Private Sub ExportItems(package As ExcelPackage)

            Dim worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add(ItemsWorkbook)

            'Add the headers

            worksheet.Cells(1, 1).Value = "Id"
            worksheet.Cells(1, 2).Value = "Type"
            worksheet.Cells(1, 3).Value = "Name"
            worksheet.Cells(1, 4).Value = "Description"
            worksheet.Cells(1, 5).Value = "Image"
            worksheet.Cells(1, 6).Value = "mruToken"
            worksheet.Cells(1, 7).Value = "IsUnique"
            worksheet.Cells(1, 8).Value = "Force"
            worksheet.Cells(1, 9).Value = "Speed"
            worksheet.Cells(1, 10).Value = "Ammo"
            worksheet.Cells(1, 11).Value = "Range"

            For i = 1 To ItemRepository.Current.Items.Count
                Dim e = ItemRepository.Current.Items(i - 1)
                worksheet.Cells(i + 1, 1).Value = e.Id
                worksheet.Cells(i + 1, 2).Value = GetStringFromType(e.Type)
                worksheet.Cells(i + 1, 3).Value = e.Name
                worksheet.Cells(i + 1, 4).Value = e.Description
                worksheet.Cells(i + 1, 5).Value = e.Image
                worksheet.Cells(i + 1, 6).Value = e.mruToken
                worksheet.Cells(i + 1, 7).Value = e.IsUnique

                Select Case e.Type
                    Case ItemType.Artillery
                        Dim artillery = DirectCast(e, Artillery)
                        worksheet.Cells(i + 1, 8).Value = artillery.Force
                        worksheet.Cells(i + 1, 9).Value = artillery.Speed
                        worksheet.Cells(i + 1, 10).Value = artillery.Ammo
                    Case ItemType.Melee
                        Dim melee = DirectCast(e, Melee)
                        worksheet.Cells(i + 1, 8).Value = melee.Force
                        worksheet.Cells(i + 1, 9).Value = melee.Speed
                        worksheet.Cells(i + 1, 11).Value = melee.Range
                End Select
            Next

        End Sub

        Private Sub ExportPerkReferences(package As ExcelPackage)

            Dim worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add(PerkRefsWorkbook)

            'Add the headers

            worksheet.Cells(1, 1).Value = "Id"
            worksheet.Cells(1, 2).Value = "PerkId"

            Dim i = 1

            For Each e In ItemRepository.Current.Items
                For Each p In e.Perks
                    worksheet.Cells(i + 1, 1).Value = e.Id
                    worksheet.Cells(i + 1, 2).Value = p.Id
                    i += 1
                Next
            Next

        End Sub

        Private Sub ExportRunes(package As ExcelPackage)
            Dim worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add(EnchantmentRefsWorkbook)

            'Add the headers

            worksheet.Cells(1, 1).Value = "Id"
            worksheet.Cells(1, 2).Value = "EnchantmentId"

            Dim i = 1

            For Each e In ItemRepository.Current.Items
                For Each p In e.Enchantments
                    worksheet.Cells(i + 1, 1).Value = e.Id
                    worksheet.Cells(i + 1, 2).Value = p.Id
                    i += 1
                Next
            Next
        End Sub

        Private Sub ExportEnchantmentReferences(package As ExcelPackage)
            Dim worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add(RunesWorkbook)

            'Add the headers

            worksheet.Cells(1, 1).Value = "Id"
            worksheet.Cells(1, 2).Value = "Rune"

            Dim i = 1

            For Each e In ItemRepository.Current.Items
                For Each p In e.Runes
                    worksheet.Cells(i + 1, 1).Value = e.Id
                    worksheet.Cells(i + 1, 2).Value = p.Name
                    i += 1
                Next
            Next
        End Sub

        Private Sub ExportPerks(package As ExcelPackage)

            Dim worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add(PerksWorkbook)

            'Add the headers

            worksheet.Cells(1, 1).Value = "Id"
            worksheet.Cells(1, 2).Value = "Description"

            For i = 1 To PerkRepository.Current.Perks.Count
                Dim e = PerkRepository.Current.Perks(i - 1)
                worksheet.Cells(i + 1, 1).Value = e.Id
                worksheet.Cells(i + 1, 2).Value = e.Description
            Next

        End Sub

        Private Sub ExportEnchantments(package As ExcelPackage)

            Dim worksheet As ExcelWorksheet = package.Workbook.Worksheets.Add(EnchantmentsWorkbook)

            'Add the headers

            worksheet.Cells(1, 1).Value = "Id"
            worksheet.Cells(1, 2).Value = "Type"
            worksheet.Cells(1, 3).Value = "Name"
            worksheet.Cells(1, 4).Value = "Description"
            worksheet.Cells(1, 5).Value = "Image"
            worksheet.Cells(1, 6).Value = "mruToken"

            For i = 1 To EnchantmentRepository.Current.Enchantments.Count
                Dim e = EnchantmentRepository.Current.Enchantments(i - 1)
                worksheet.Cells(i + 1, 1).Value = e.Id
                worksheet.Cells(i + 1, 2).Value = GetStringFromType(e.Type)
                worksheet.Cells(i + 1, 3).Value = e.Name
                worksheet.Cells(i + 1, 4).Value = e.Description
                worksheet.Cells(i + 1, 5).Value = e.Image
                worksheet.Cells(i + 1, 6).Value = e.mruToken
            Next

        End Sub

    End Class

End Namespace
