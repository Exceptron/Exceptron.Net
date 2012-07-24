using System;
using System.Collections.Generic;
using System.Linq;
using Exceptron.Client.Message;
using FluentAssertions;
using NUnit.Framework;

namespace Exceptron.Client.Tests.ExceptionClientTests
{
    [TestFixture]
    public class ExceptionClientFixture_ConvertToFrames
    {

        [Test]
        public void should_be_able_to_submit_exception_thrown_from_static_method()
        {
            ThrowsExceptionFromStaticMethod();
        }

        [Test]
        public void should_be_able_to_submit_exception_thrown_from_method_with_args()
        {
            ThrowsExceptionFromMethodWithArgs("", 0);
        }


        private static List<Frame> LambdaException()
        {
            List<Frame> frames = null;

            try
            {
                var testStrings = new string[] { "Not A number" };
                testStrings.Select(d => Convert.ToInt32(d)).ToList();
            }
            catch (Exception e)
            {
                frames = ExceptionClient.ConvertToFrames(e);
            }

            AssertOnFrames(frames);

            return frames;
        }

        private delegate void ExceptionDelegate();

        private static List<Frame> DelegateException()
        {
            List<Frame> frames = null;

            try
            {
                ExceptionDelegate exceptionDelegate = () => Convert.ToInt32("NOT A NUMBER");
                exceptionDelegate();
            }
            catch (Exception e)
            {
                frames = ExceptionClient.ConvertToFrames(e);
            }

            AssertOnFrames(frames);

            return frames;
        }



        private static void AssertOnFrames(List<Frame> frames)
        {
            frames.Should().NotBeEmpty();
            frames.Should().OnlyContain(c => c.ln >= 0);
            frames.Should().NotContain(c => string.IsNullOrEmpty(c.m));
            frames.Should().NotContain(c => string.IsNullOrEmpty(c.c));
            frames.Should().NotContain(c => c.m.StartsWith("object", StringComparison.CurrentCultureIgnoreCase), "Method contains return type");
            frames.Should().NotContain(c => c.m.StartsWith("void", StringComparison.CurrentCultureIgnoreCase), "Method contains return type");
            frames.Should().NotContain(c => c.m.StartsWith(" ", StringComparison.CurrentCultureIgnoreCase));
            frames.Should().NotContain(c => c.m.StartsWith(",", StringComparison.CurrentCultureIgnoreCase));
            frames.Should().NotContain(c => c.m.EndsWith(" ", StringComparison.CurrentCultureIgnoreCase));
            frames.Should().NotContain(c => c.m.EndsWith(",", StringComparison.CurrentCultureIgnoreCase));
            
            frames.Select(c => c.i).Should().OnlyHaveUniqueItems();
            frames.Min(c => c.i).Should().Be(0);
            frames.Max(c => c.i).Should().Be(frames.Count-1);

            //Should contain at least one frame with full location
            frames.Should().Contain(c => !string.IsNullOrEmpty(c.fn));
            frames.Should().Contain(c => c.ln > 0);
        }

        private static object ThrowsExceptionFromStaticMethod()
        {
            LambdaException();
            DelegateException();
            return null;
        }

        private static object ThrowsExceptionFromMethodWithArgs(string stringArg, int intArg)
        {
            LambdaException();
            DelegateException();
            return null;
        }
    }

}
