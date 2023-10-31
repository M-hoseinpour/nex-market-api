using AutoMapper;

namespace market.Models.DTO.Product;

public class UpdateProductInput : AddProductInput
{
    public int Id { get; set; }
}