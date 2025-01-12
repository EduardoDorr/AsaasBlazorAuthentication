﻿using AsaasBlazorAuthentication.Common.Results;
using AsaasBlazorAuthentication.Common.Entities;
using AsaasBlazorAuthentication.Domain.Enrollments;

namespace AsaasBlazorAuthentication.Domain.EnrollmentPayments;

public sealed class EnrollmentPayment : BaseEntity
{
    public Guid EnrollmentId { get; private set; }
    public string Message { get; private set; }
    public decimal Value { get; private set; }
    public EnrollmentPaymentStatus Status { get; private set; }
    public DateTime DueDate { get; private set; }
    public string PaymentLink { get; private set; }
    public string PaymentId { get; private set; }
    public DateTime? ProcessedAt { get; private set; }

    public Enrollment Enrollment { get; private set; }

    protected EnrollmentPayment() { }

    private EnrollmentPayment(Guid enrollmentId, string message, decimal value, DateTime dueDate, string paymentLink, string paymentId)
    {
        EnrollmentId = enrollmentId;
        Message = message;
        Value = value;
        DueDate = dueDate;
        PaymentLink = paymentLink;
        PaymentId = paymentId;
        Status = EnrollmentPaymentStatus.Pending;
    }

    public static Result<EnrollmentPayment> Create(
        Guid enrollmentId,
        string message,
        decimal value,
        DateTime dueDate,
        string paymentLink,
        string paymentId)
    {
        var enrollmentPayment =
            new EnrollmentPayment(
                enrollmentId,
                message,
                value,
                dueDate,
                paymentLink,
                paymentId);

        return Result.Ok(enrollmentPayment);
    }

    public void SetStatus(EnrollmentPaymentStatus status)
    {
        switch (status)
        {
            case EnrollmentPaymentStatus.Success:
                SetSuccessStatus();
                break;
            case EnrollmentPaymentStatus.Fail:
                SetFailStatus();
                break;
            case EnrollmentPaymentStatus.Overdue:
                SetOverdueStatus();
                break;
            default:
                break;
        }
    }

    private void SetSuccessStatus()
    {
        Status = EnrollmentPaymentStatus.Success;
        ProcessedAt = DateTime.Now;
    }

    private void SetFailStatus()
    {
        Status = EnrollmentPaymentStatus.Fail;
        ProcessedAt = DateTime.Now;
    }

    private void SetOverdueStatus() =>
        Status = EnrollmentPaymentStatus.Overdue;
}