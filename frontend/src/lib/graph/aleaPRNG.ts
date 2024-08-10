// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================
// @see https://github.com/macmcmeans/aleaPRNG/blob/master/aleaPRNG-1.1.js
// ============================================================================
// Original work copyright © 2010 Johannes Baagøe, under MIT license
// This is a derivative work copyright (c) 2017-2020, W. Mac" McMeans, under BSD license.
// ============================================================================
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 1. Redistributions of source code must retain the above copyright notice,
//		this list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright notice,
//		this list of conditions and the following disclaimer in the documentation
//		and/or other materials provided with the distribution.
// 3. Neither the name of the copyright holder nor the names of its contributors
//		may be used to endorse or promote products derived from this software
//		without specific prior written permission.
// ============================================================================
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
// IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
// ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// ============================================================================

export default class AleaPRNG {
	private s0: number;
	private s1: number;
	private s2: number;
	private c: number;
	private initialArgs: string[];

	constructor(...args: string[]) {
		this.c = 1;
		this.s0 = 0;
		this.s1 = 0;
		this.s2 = 0;
		this.initialArgs = [];
		this.init(args);
	}

	//===========================================================================

	/**
	 * Returns a random number between 0 and 1
	 */
	public random() {
		const { s0, s1, s2, c } = this;
		const t = 2091639 * s0 + c * 2.3283064365386963e-10; // 2^-32

		this.s0 = s1;
		this.s1 = s2;
		return (this.s2 = t - (this.c = t | 0));
	}

	/**
	 * Return a random fraction between 0 and 1
	 */
	public fract53() {
		return this.random() + ((this.random() * 0x200000) | 0) * 1.1102230246251565e-16; // 2^-53
	}

	/**
	 * Returns a random integer between 0 and 2^32
	 */
	public int32() {
		return this.random() * 0x100000000; // 2^32
	}

	/**
	 * Cycles the generator a number of times
	 */
	public cycle(cycles: number = 1) {
		for (let i = 0; i < +cycles; i++) {
			this.random();
		}
	}

	/**
	 * Returns a random number between min and max
	 * @param min The lower bound
	 * @param max The upper bound
	 * @returns A random number between min and max
	 */
	public range(min: number = 0, max: number = 1) {
		if (min > max) [min, max] = [max, min];
		if (this._isInteger(min) && this._isInteger(max)) {
			return Math.floor(this.random() * (max - min + 1)) + min;
		} else {
			return this.random() * (max - min) + min;
		}
	}

	/**
	 * Returns a number between either min and max or max and min but not any other number
	 * @param minMax The maximum value of the minimum range
	 * @param minMin The minimum value of the minimum range
	 * @param maxMax The maximum value of the maximum range
	 * @param maxMin The minimum value of the maximum range
	 *
	 * @example (-45, -20, 20, 45) will return a number between -45 and -20 or 20 and 45 but not between -20 and 20
	 */
	public rangeBoundary(minMax: number, minMin: number, maxMax: number, maxMin: number) {
		if (minMax > minMin) [minMax, minMin] = [minMin, minMax];
		if (maxMax > maxMin) [maxMax, maxMin] = [maxMin, maxMax];

		const min = this.range(minMax, minMin);
		const max = this.range(maxMax, maxMin);

		return this.range(min, max);
	}

	/** Restarts the generator with the initial seed */
	public restart() {
		this.init(this.initialArgs);
	}

	/** Seeds the generator with a new seed */
	public seed(...args: any[]) {
		this.init(args);
	}

	//===========================================================================

	private init(internalSeed: string[]) {
		this.s0 = this.mash(" ");
		this.s1 = this.mash(" ");
		this.s2 = this.mash(" ");

		for (let i = 0; i < internalSeed.length; i++) {
			const currentSeed = internalSeed[i]!;

			this.s0 -= this.mash(currentSeed);
			if (this.s0 < 0) {
				this.s0 += 1;
			}

			this.s1 -= this.mash(currentSeed);
			if (this.s1 < 0) {
				this.s1 += 1;
			}

			this.s2 -= this.mash(currentSeed);
			if (this.s2 < 0) {
				this.s2 += 1;
			}
		}
	}

	private mash(data: string) {
		// 0xefc8249d
		let n = 4022871197;

		for (let i = 0, l = data.length; i < l; i++) {
			n += data.charCodeAt(i);

			let h = 0.02519603282416938 * n;

			n = h >>> 0;
			h -= n;
			h *= n;
			n = h >>> 0;
			h -= n;
			n += h * 4294967296; // 0x100000000      2^32
		}

		return (n >>> 0) * 2.3283064365386963e-10; // 2^-32
	}

	private _isInteger(_int: number) {
		return parseInt(_int.toString(), 10) === _int;
	}
}
