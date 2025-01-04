using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MinimalAPIDemo.Data;
using MinimalAPIDemo.Mappers;
using MinimalAPIDemo.Models;
using MinimalAPIDemo.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//register automapper
builder.Services.AddAutoMapper(typeof(MapConfig));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/coupons", (ILogger<Program> _logger, IMapper _mapper) =>
{
    _logger.Log(LogLevel.Information, "Getting all coupons");
    var coupons = CouponStore.couponList;

    List<CouponCreateResponse> couponsResponse = _mapper.Map<List<CouponCreateResponse>>(coupons);

    return Results.Ok(couponsResponse);

}).WithName("GetCoupons").Produces<IEnumerable<CouponCreateResponse>>(200);

app.MapGet("/api/coupons/{id:int}", (int id) =>
{
    var coupon = CouponStore.couponList.FirstOrDefault(c => c.Id == id);

    if (coupon != null)
        return Results.Ok(coupon);

    return Results.BadRequest("Coupon does not exist");

}).WithName("GetCoupon").Produces<Coupon>(200).Produces(400);

app.MapPost("/api/coupons", (IMapper _mapper, [FromBody] CouponCreateDTO request) =>
{
    if (string.IsNullOrEmpty(request.Name))
    {
        return Results.BadRequest("Invalid Coupon");
    }

    if (CouponStore.couponList.FirstOrDefault(c => c.Name.ToLower() == request.Name.ToLower()) != null)
    {
        return Results.BadRequest("Coupon already exists");
    }

    Coupon coupon = _mapper.Map<Coupon>(request);

    var couponIds = CouponStore.couponList.Select(c => c.Id).ToList();
    coupon.Id = couponIds.Last() + 1;
    CouponStore.couponList.Add(coupon);

    CouponCreateResponse couponResponse = _mapper.Map<CouponCreateResponse>(coupon);

    return Results.CreatedAtRoute("GetCoupon", new { id = coupon.Id }, couponResponse);

}).WithName("CreateCoupon").Accepts<CouponCreateDTO>("application/json").Produces<CouponCreateResponse>(201).Produces(400);

app.UseHttpsRedirection();

app.Run();
