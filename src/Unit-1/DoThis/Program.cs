﻿using System;
using Akka.Actor;

namespace WinTail
{
    #region Program
    class Program
    {
        public static ActorSystem MyActorSystem;

        // Program.cs
        static void Main(string[] args)
        {
            // initialize MyActorSystem
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            // nothing here where our actors used to be!
            Props consoleWriterProps = Props.Create<ConsoleWriterActor>();
            ActorRef consoleWriterActor = MyActorSystem.ActorOf(consoleWriterProps, "consoleWriterActor");

            var tailCoordinatorProps = Props.Create(() => new TailCoordinatorActor());
            ActorRef tailCoordinatorActor = MyActorSystem.ActorOf(tailCoordinatorProps, "tailCoordinatorActor");

            var fileValidatorActorProps = Props.Create(() => new FileValidatorActor(consoleWriterActor));
            ActorRef validationActor = MyActorSystem.ActorOf(fileValidatorActorProps, "validationActor");

            Props consoleReaderProps = Props.Create<ConsoleReaderActor>();
            ActorRef consoleReaderActor = MyActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");

            // tell console reader to begin
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.AwaitTermination();
        }

    }
    #endregion
}
