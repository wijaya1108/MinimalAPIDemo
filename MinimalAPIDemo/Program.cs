using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MinimalAPIDemo.Data;
using MinimalAPIDemo.Mappers;
using MinimalAPIDemo.Models;
using MinimalAPIDemo.Models.DTO;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//register automapper
builder.Services.AddAutoMapper(typeof(MapConfig));

//register fluent validator
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

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

    APIResponse response = new()
    {
        Success = true,
        Result = couponsResponse,
        StatusCode = HttpStatusCode.OK
    };

    return Results.Ok(response);

}).WithName("GetCoupons").Produces<APIResponse>(200);


app.MapGet("/api/coupons/{id:int}", (IMapper _mapper, int id) =>
{
    var coupon = CouponStore.couponList.FirstOrDefault(c => c.Id == id);

    CouponCreateResponse couponResponse = _mapper.Map<CouponCreateResponse>(coupon);

    APIResponse response = new APIResponse();

    if (coupon != null)
    {
        response.Success = true;
        response.Result = couponResponse;
        response.StatusCode = HttpStatusCode.OK;

        return Results.Ok(response);
    }

    response.ErrorMessages.Add("Cannot find the requested coupon");

    return Results.BadRequest(response);

}).WithName("GetCoupon").Produces<APIResponse>(200);


app.MapPost("/api/coupons", async (IMapper _mapper, IValidator<CouponCreateDTO> _validator, [FromBody] CouponCreateDTO request) =>
{
    APIResponse response = new APIResponse();

    var validationResult = await _validator.ValidateAsync(request);

    if (!validationResult.IsValid)
    {
        List<string> errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

        response.ErrorMessages = errorMessages;
        return Results.BadRequest(response);
    }

    if (CouponStore.couponList.FirstOrDefault(c => c.Name.ToLower() == request.Name.ToLower()) != null)
    {
        response.ErrorMessages.Add("Coupon already exists");
        return Results.BadRequest(response);
    }

    Coupon coupon = _mapper.Map<Coupon>(request);

    var couponIds = CouponStore.couponList.Select(c => c.Id).ToList();
    coupon.Id = couponIds.Last() + 1;
    CouponStore.couponList.Add(coupon);

    CouponCreateResponse couponResponse = _mapper.Map<CouponCreateResponse>(coupon);

    response.Success = true;
    response.StatusCode = HttpStatusCode.Created;
    response.Result = couponResponse;

    return Results.Ok(response);
    //return Results.CreatedAtRoute("GetCoupon", new { id = coupon.Id }, couponResponse);

}).WithName("CreateCoupon").Accepts<CouponCreateDTO>("application/json").Produces<APIResponse>(201);


//update endpoint
app.MapPut("/api/coupons", async (IValidator<CouponUpdateRequest> _validator, IMapper _mapper, [FromBody] CouponUpdateRequest request) =>
{
    APIResponse response = new();

    var validationResult = await _validator.ValidateAsync(request);

    if (!validationResult.IsValid)
    {
        List<string> errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        response.ErrorMessages = errorMessages;
        return Results.BadRequest(response);
    }

    var existingCoupon = CouponStore.couponList.FirstOrDefault(c => c.Id == request.Id);

    if (existingCoupon == null)
    {
        response.ErrorMessages.Add("Requested coupon does not exist");
        return Results.BadRequest(response);
    }

    existingCoupon.Name = request.Name;
    existingCoupon.Percent = request.Percent;
    existingCoupon.IsActive = request.IsActive;
    existingCoupon.LastUpdated = DateTime.UtcNow;

    var couponResponse = _mapper.Map<CouponCreateResponse>(existingCoupon);

    response.Success = true;
    response.StatusCode = HttpStatusCode.OK;
    response.Result = couponResponse;

    return Results.Ok(response);

}).Accepts<CouponUpdateRequest>("application/json").Produces<APIResponse>(200);

app.UseHttpsRedirection();

app.Run();
