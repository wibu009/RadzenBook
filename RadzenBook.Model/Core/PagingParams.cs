﻿namespace FirstBlazorProject_BookStore.Model.Core;

public class PagingParams
{
    private const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    public string? SearchValue { get; set; }
    public string? SortOrder { get; set; }
}