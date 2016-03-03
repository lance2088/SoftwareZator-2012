// *****************************************************************************
// 
//  � Veler Software 2012. All rights reserved.
//  The current code and the associated software are the proprietary 
//  information of Etienne Baudoux from Veler Software and are
//  supplied subject to licence terms.
// 
//  www.velersoftware.com
// *****************************************************************************


using System;

using Mono.Collections.Generic;

namespace Mono.Cecil.Cil {

	public sealed class ILProcessor {

		readonly MethodBody body;
		readonly InstructionCollection instructions;

		public MethodBody Body {
			get { return body; }
		}

		internal ILProcessor (MethodBody body)
		{
			this.body = body;
			this.instructions = (InstructionCollection)body.Instructions;
		}

		public Instruction Create (OpCode opcode)
		{
			return Instruction.Create (opcode);
		}

		public Instruction Create (OpCode opcode, TypeReference type)
		{
			return Instruction.Create (opcode, type);
		}

		public Instruction Create (OpCode opcode, CallSite site)
		{
			return Instruction.Create (opcode, site);
		}

		public Instruction Create (OpCode opcode, MethodReference method)
		{
			return Instruction.Create (opcode, method);
		}

		public Instruction Create (OpCode opcode, FieldReference field)
		{
			return Instruction.Create (opcode, field);
		}

		public Instruction Create (OpCode opcode, string value)
		{
			return Instruction.Create (opcode, value);
		}

		public Instruction Create (OpCode opcode, sbyte value)
		{
			return Instruction.Create (opcode, value);
		}

		public Instruction Create (OpCode opcode, byte value)
		{
			if (opcode.OperandType == OperandType.ShortInlineVar)
				return Instruction.Create (opcode, body.Variables [value]);

			if (opcode.OperandType == OperandType.ShortInlineArg)
				return Instruction.Create (opcode, body.GetParameter (value));

			return Instruction.Create (opcode, value);
		}

		public Instruction Create (OpCode opcode, int value)
		{
			if (opcode.OperandType == OperandType.InlineVar)
				return Instruction.Create (opcode, body.Variables [value]);

			if (opcode.OperandType == OperandType.InlineArg)
				return Instruction.Create (opcode, body.GetParameter (value));

			return Instruction.Create (opcode, value);
		}

		public Instruction Create (OpCode opcode, long value)
		{
			return Instruction.Create (opcode, value);
		}

		public Instruction Create (OpCode opcode, float value)
		{
			return Instruction.Create (opcode, value);
		}

		public Instruction Create (OpCode opcode, double value)
		{
			return Instruction.Create (opcode, value);
		}

		public Instruction Create (OpCode opcode, Instruction target)
		{
			return Instruction.Create (opcode, target);
		}

		public Instruction Create (OpCode opcode, Instruction [] targets)
		{
			return Instruction.Create (opcode, targets);
		}

		public Instruction Create (OpCode opcode, VariableDefinition variable)
		{
			return Instruction.Create (opcode, variable);
		}

		public Instruction Create (OpCode opcode, ParameterDefinition parameter)
		{
			return Instruction.Create (opcode, parameter);
		}

		public void Emit (OpCode opcode)
		{
			Append (Create (opcode));
		}

		public void Emit (OpCode opcode, TypeReference type)
		{
			Append (Create (opcode, type));
		}

		public void Emit (OpCode opcode, MethodReference method)
		{
			Append (Create (opcode, method));
		}

		public void Emit (OpCode opcode, CallSite site)
		{
			Append (Create (opcode, site));
		}

		public void Emit (OpCode opcode, FieldReference field)
		{
			Append (Create (opcode, field));
		}

		public void Emit (OpCode opcode, string value)
		{
			Append (Create (opcode, value));
		}

		public void Emit (OpCode opcode, byte value)
		{
			Append (Create (opcode, value));
		}

		public void Emit (OpCode opcode, sbyte value)
		{
			Append (Create (opcode, value));
		}

		public void Emit (OpCode opcode, int value)
		{
			Append (Create (opcode, value));
		}

		public void Emit (OpCode opcode, long value)
		{
			Append (Create (opcode, value));
		}

		public void Emit (OpCode opcode, float value)
		{
			Append (Create (opcode, value));
		}

		public void Emit (OpCode opcode, double value)
		{
			Append (Create (opcode, value));
		}

		public void Emit (OpCode opcode, Instruction target)
		{
			Append (Create (opcode, target));
		}

		public void Emit (OpCode opcode, Instruction [] targets)
		{
			Append (Create (opcode, targets));
		}

		public void Emit (OpCode opcode, VariableDefinition variable)
		{
			Append (Create (opcode, variable));
		}

		public void Emit (OpCode opcode, ParameterDefinition parameter)
		{
			Append (Create (opcode, parameter));
		}

		public void InsertBefore (Instruction target, Instruction instruction)
		{
			if (target == null)
				throw new ArgumentNullException ("target");
			if (instruction == null)
				throw new ArgumentNullException ("instruction");

			var index = instructions.IndexOf (target);
			if (index == -1)
				throw new ArgumentOutOfRangeException ("target");

			instructions.Insert (index, instruction);
		}
        public void InsertBefore(int targetIndex, Instruction instruction)
        {
            if (instruction == null)
                throw new ArgumentNullException("instruction");
            if (targetIndex > instructions.Count || targetIndex < 0)
                throw new ArgumentOutOfRangeException("targetIndex");

            instructions.Insert(targetIndex, instruction);
        }

        public void InsertAfter(Instruction target, Instruction instruction)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (instruction == null)
                throw new ArgumentNullException("instruction");

            var index = instructions.IndexOf(target);
            if (index == -1)
                throw new ArgumentOutOfRangeException("target");

            instructions.Insert(index + 1, instruction);
        }
        public void InsertAfter(int targetIndex, Instruction instruction)
        {
            if (instruction == null)
                throw new ArgumentNullException("instruction");
            if (targetIndex > instructions.Count || targetIndex < 0)
                throw new ArgumentOutOfRangeException("targetIndex");

            instructions.Insert(targetIndex + 1, instruction);
        }

		public void Append (Instruction instruction)
		{
			if (instruction == null)
				throw new ArgumentNullException ("instruction");

			instructions.Add (instruction);
		}

        public void Replace(Instruction target, Instruction instruction)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (instruction == null)
                throw new ArgumentNullException("instruction");

            int index = instructions.IndexOf(target);
            instructions[index] = instruction;
            if (index != 0) instructions[index - 1].next = instruction;
            if (index != instructions.Count - 1) instructions[index + 1].previous = instruction;
            foreach (Instruction inst in instructions.instReferences)
            {
                if (inst.Operand is Instruction && inst.Operand == target)
                    inst.Operand = instruction;
                else if (inst.Operand is Instruction[])
                {
                    Instruction[] s = inst.Operand as Instruction[];
                    for (int i = 0; i < s.Length; i++)
                        if (s[i] == target)
                            s[i] = instruction;
                    inst.Operand = s;
                }
            }

            if (body.exceptions != null)
            {
                foreach (ExceptionHandler eh in body.exceptions)
                {
                    if (eh.TryStart == target) eh.TryStart = instruction;
                    if (eh.TryEnd == target) eh.TryEnd = instruction;
                    if (eh.HandlerStart == target) eh.HandlerStart = instruction;
                    if (eh.HandlerEnd == target) eh.HandlerEnd = instruction;
                    if (eh.FilterStart == target) eh.FilterStart = instruction;
                }
            }
        }
        public void Replace(int targetIndex, Instruction instruction)
        {
            if (targetIndex > instructions.Count || targetIndex < 0)
                throw new ArgumentOutOfRangeException("target");
            if (instruction == null)
                throw new ArgumentNullException("instruction");

            Instruction target = instructions[targetIndex];
            instructions[targetIndex] = instruction;
            if (targetIndex != 0) instructions[targetIndex - 1].next = instruction;
            if (targetIndex != instructions.Count - 1) instructions[targetIndex + 1].previous = instruction;
            foreach (Instruction inst in instructions.instReferences)
            {
                if (inst.Operand is Instruction && inst.Operand == target)
                    inst.Operand = instruction;
                else if (inst.Operand is Instruction[])
                {
                    Instruction[] s = inst.Operand as Instruction[];
                    for (int i = 0; i < s.Length; i++)
                        if (s[i] == target)
                            s[i] = instruction;
                    inst.Operand = s;
                }
            }

            if (body.exceptions != null)
            {
                foreach (ExceptionHandler eh in body.exceptions)
                {
                    if (eh.TryStart == target) eh.TryStart = instruction;
                    if (eh.TryEnd == target) eh.TryEnd = instruction;
                    if (eh.HandlerStart == target) eh.HandlerStart = instruction;
                    if (eh.HandlerEnd == target) eh.HandlerEnd = instruction;
                    if (eh.FilterStart == target) eh.FilterStart = instruction;
                }
            }
        }

		public void Remove (Instruction instruction)
		{
			if (instruction == null)
				throw new ArgumentNullException ("instruction");

			if (!instructions.Remove (instruction))
				throw new ArgumentOutOfRangeException ("instruction");
		}
		public void Remove (int targetIndex)
		{
            instructions.RemoveAt (targetIndex);
		}
	}
}