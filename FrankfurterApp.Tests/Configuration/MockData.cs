using System;
using FrankfurterApp.Authentication;

namespace FrankfurterApp.Tests.Configuration
{
    public static class MockData
    {
        public static string TestUserRole = UserRole.User;
        public static DateTime MockDate = DateTime.UtcNow.Date;
        public static readonly string MockCurrencyEur = "EUR";
        public static readonly string MockCurrencyUsd = "USD";
        public static readonly decimal MockRateEurToUsd = 2M;
        public static readonly decimal MockRateUsdToEur = 0.5M;
        public static DateTime MockStartDate = DateTime.UtcNow.Date.AddDays(-100);
        public static DateTime MockEndDate = DateTime.UtcNow.Date.AddDays(-20);
        public static DateTime MockCustomDate = DateTime.UtcNow.Date.AddDays(-7);
    }
}