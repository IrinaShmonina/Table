using System;
using System.Collections.Generic;

namespace model
{
    public class Cell : IObserver, IObservable
    {
        public String originalText;
        public String text;
        public List<IObserver> observers;
        public List<IObservable> observerables;

        public Cell(String text, String parsedText)
        {
            observers = new List<IObserver>();
            observerables = new List<IObservable>();
            setText(text, parsedText);
        }

        public void setText(String originalText)
        {
            foreach (var e in observerables)
                if (e is Cell)
                    (e as Cell).observers.Remove(this);
            observerables = new List<IObservable>();
            this.originalText = originalText;
            var tuple = new TextParser().parseText(text);
            this.text = tuple.Item2;
            foreach (var e in tuple.Item1)
            {
                e.observers.Add(this);
            }
            this.NotifayObservers();
            this.text = text;
        }

        public override string ToString()
        {
            return "Cell(" + text + ") ";
        }

        public void Notify()
        {
            setText(this.originalText, new TextParser().parseText(this.originalText).Item2);
        }

        public void NotifayObservers()
        {
            foreach (var e in this.observers)
                e.Notify();
        }
    }
}