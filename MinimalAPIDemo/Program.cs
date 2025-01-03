using Microsoft.AspNetCore.Mvc;
using MinimalAPIDemo.Data;
using MinimalAPIDemo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/coupons", () =>
{
    var coupons = CouponStore.couponList;
    return Results.Ok(coupons);

}).WithName("GetCoupons").Produces<IEnumerable<Coupon>>(200);

app.MapGet("/api/coupons/{id:int}", (int id) =>
{
    var coupon = CouponStore.couponList.FirstOrDefault(c => c.Id == id);

    if (coupon != null)
        return Results.Ok(coupon);

    return Results.BadRequest("Coupon does not exist");

}).WithName("GetCoupon").Produces<Coupon>(200).Produces(400);

app.MapPost("/api/coupons", ([FromBody] Coupon coupon) =>
{
    if (coupon.Id != 0 || string.IsNullOrEmpty(coupon.Name))
    {
        return Results.BadRequest("Invalid Coupon");
    }

    if (CouponStore.couponList.FirstOrDefault(c => c.Name.ToLower() == coupon.Name.ToLower()) != null)
    {
        return Results.BadRequest("Coupon already exists");
    }

    var couponIds = CouponStore.couponList.Select(c => c.Id).ToList();
    coupon.Id = couponIds.Last() + 1;
    CouponStore.couponList.Add(coupon);


    return Results.CreatedAtRoute("GetCoupon", new { id = coupon.Id }, coupon);

}).WithName("CreateCoupon").Accepts<Coupon>("application/json").Produces<Coupon>(201).Produces(400);

app.UseHttpsRedirection();

app.Run();
