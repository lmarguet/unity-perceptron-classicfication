﻿using System;
 using System.Linq;
 using UnityEngine;
 using Random = System.Random;

namespace Assets.Scripts
{
    [Serializable]
    public class TrainingSet
    {
        public double[] Input { get; private set; }
        public double Output{ get; private set; }

        public TrainingSet(double[] input, double output){
            Input = input;
            Output = output;
        }
    }


    public class Perceptron: MonoBehaviour
    {
        [HideInInspector]
        public TrainingSet[] trainingSet = {
            new TrainingSet(new double[]{0.5f, 1}, 1), 
            new TrainingSet(new double[]{0, 0.8f}, 1), 
            new TrainingSet(new double[]{0.2f, 1f}, 1), 
            new TrainingSet(new double[]{0.1f, 0.5f}, 1), 
            new TrainingSet(new double[]{0.3f, 0.8f}, 1), 
            new TrainingSet(new double[]{0.5f, 0.9f}, 1), 
            new TrainingSet(new double[]{0.8f, 1}, 1), 
            new TrainingSet(new double[]{0.1f, 0}, 0), 
            new TrainingSet(new double[]{0.5f, 0.3f}, 0), 
            new TrainingSet(new double[]{0.6f, 0.3f}, 0), 
            new TrainingSet(new double[]{0.6f, 0.1f}, 0), 
            new TrainingSet(new double[]{0.7f, 0}, 0), 
            new TrainingSet(new double[]{0.9f, 0}, 0)
            
        };

        public SimpleGrapher Graph;

        private readonly double[] weights;
        private double bias;
        private double totalError;


        public Perceptron()
        {
            weights = new double[2];
            
        }

        void Start()
        {
           DrawAllPoints();
           Train(trainingSet, 10);
            
            Graph.DrawRay(
                (float)(-(bias / weights[1]) / (bias / weights[0])),
                (float)(-bias / weights[1]),
                Color.red
            );
        }

        public void Train(TrainingSet[] set,  int epochs)
        {
            trainingSet = set;

            InitialiseWeights();

            for (var i = 0; i < epochs; i++)
            {
                totalError = 0;

                for (var j = 0; j < trainingSet.Length; j++)
                {
                    UpdateWeights(j);
                }
                
                Debug.Log(string.Format("Epoch:{0} Total errors:{1}", i, totalError));
                Debug.Log("##########################################");
            }
        }

        void DrawAllPoints()
        {
            foreach (var set in trainingSet){
                Graph.DrawPoint(
                    (float)set.Input[0],    
                    (float)set.Input[1],
                    set.Output == 0 ? Color.magenta : Color.green
                );
            }
        }

        public double CalculateOutput(double input1, double input2)
        {
            var product = DotProductBias(
                weights,
                new double[] { input1, input2 }
            );

            return (product > 0) ? 1 : 0;
        }


        void InitialiseWeights()
        {
            for (var i = 0; i < weights.Length; i++)
            {
                weights[i] = RandomRange(-1.0f, 1.0f);
            }

            bias = RandomRange(-1.0f, 1.0f);
        }


        void UpdateWeights(int index)
        {
            var currentSet = trainingSet[index];
            var output = CalculateOutput(currentSet);
            var error = currentSet.Output - output;

            totalError += Math.Abs((float)error);

            for (var i = 0; i < weights.Length; i++)
            {
                weights[i] += error * currentSet.Input[i];
            }

            bias += error;

//            LogInfos(currentSet, output, error);
        }


        private double CalculateOutput(TrainingSet set)
        {
            var product = DotProductBias(
                weights, 
                set.Input
            );

            return (product > 0) ? 1 : 0;
        }


        private double DotProductBias(double[]weights, double[] inputs)
        {
            if (inputs == null) throw new ArgumentNullException("inputs");
            if((weights == null || inputs == null)
               || (weights.Length != inputs.Length))
            {
                return -1;
            }

            var product = weights.Select((t, i) => t * inputs[i])
                                 .Sum();

            product += bias;

            return product;
        }

        private static double RandomRange(float lower, float upper)
        {
            var random = new Random();
            return random.NextDouble() * (upper - lower) + (lower - 0);
        }

        private void LogInfos(TrainingSet currentSet, double output, double error)
        {
            var log = string.Format("Inputs: {0} {1}\t Weights: {2}\t{3}\tBias: {4} \tOutput: {5}\t Desired: {6}\tError: {7}",
                currentSet.Input[0],
                currentSet.Input[1],
                weights[0],
                weights[1],
                bias,
                output,
                currentSet.Output,
                error);
            Debug.Log(log);
        }



    }
}
