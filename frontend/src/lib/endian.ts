// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

export class EndianReader {
	private dataView: DataView;
	private offset: number;

	constructor(buffer: ArrayBuffer, offset: number = 0) {
		this.dataView = new DataView(buffer);
		this.offset = offset;
	}

	readInt32(): number {
		const value = this.dataView.getInt32(this.offset, true); // true for little-endian
		this.offset += 4;
		return value;
	}

	readUInt16(): number {
		const value = this.dataView.getUint16(this.offset, true); // true for little-endian
		this.offset += 2;
		return value;
	}

	readULong(): number {
		const value = this.dataView.getBigUint64(this.offset, true); // true for little-endian
		this.offset += 8;
		return Number(value); // Convert BigInt to number
	}

	readAlignment(alignment: number): void {
		this.offset = readPadding(this.offset, alignment);
	}

	readCString(): string | null {
		let start = this.offset;
		while (this.dataView.getUint8(this.offset) !== 0) {
			this.offset++;
			if (this.offset >= this.dataView.byteLength) {
				throw new Error("Null-terminated string not found.");
			}
		}
		const value = new TextDecoder().decode(
			this.dataView.buffer.slice(start, this.offset),
		);
		this.offset++; // Skip null terminator
		return value;
	}

	readGuid(): string {
		const parts: string[] = [];
		for (let i = 0; i < 16; i++) {
			parts.push(this.dataView.getUint8(this.offset).toString(16).padStart(2, "0"));
			this.offset++;
		}
		return `${parts.slice(0, 4).join("")}-${parts.slice(4, 6).join("")}-${parts
			.slice(6, 8)
			.join("")}-${parts.slice(8, 10).join("")}-${parts.slice(10).join("")}`;
	}

	readBool(): boolean {
		const value = this.dataView.getUint8(this.offset);
		this.offset++;
		return value !== 0;
	}
}

// ============================================================================

export class EndianWriter {
	private dataView: DataView;
	private offset: number;

	constructor(buffer: ArrayBuffer, offset: number = 0) {
		this.dataView = new DataView(buffer);
		this.offset = offset;
	}

	writeInt32(value: number): void {
		this.dataView.setInt32(this.offset, value, true); // true for little-endian
		this.offset += 4;
	}

	writeUInt16(value: number): void {
		this.dataView.setUint16(this.offset, value, true); // true for little-endian
		this.offset += 2;
	}

	writeULong(value: number): void {
		this.dataView.setBigUint64(this.offset, BigInt(value), true); // true for little-endian
		this.offset += 8;
	}

	writePadding(padding: number): void {
		const paddingSize = writePadding(this.offset, padding);
		for (let i = 0; i < paddingSize; i++) {
			this.dataView.setUint8(this.offset, 0);
			this.offset++;
		}
	}

	/**
	 * Write the
	 * @param value
	 */
	writeCString(value: string): void {
		const encoded = new TextEncoder().encode(value);
		encoded.forEach((byte) => {
			this.dataView.setUint8(this.offset, byte);
			this.offset++;
		});
		this.dataView.setUint8(this.offset, 0); // Null terminator
		this.offset++;
	}

	write(value: number | boolean): void {
		if (typeof value === "number") {
			this.dataView.setFloat64(this.offset, value, true); // true for little-endian
			this.offset += 8;
		} else if (typeof value === "boolean") {
			this.dataView.setUint8(this.offset, value ? 1 : 0);
			this.offset++;
		} else {
			throw new Error("Unsupported type for write operation.");
		}
	}
}

// ============================================================================

/**
 *
 * @param position
 * @param alignment
 * @returns
 */
export function readPadding(position: number, alignment: number): number {
	if (alignment <= 0 || (alignment & (alignment - 1)) !== 0) {
		throw new Error("Alignment must be a power of 2.");
	}
	return (position + (alignment - 1)) & ~(alignment - 1);
}

/**
 *
 * @param position
 * @param alignment
 * @returns
 */
export function writePadding(position: number, alignment: number): number {
	if (alignment <= 0 || (alignment & (alignment - 1)) !== 0) {
		throw new Error("Alignment must be a power of 2.");
	}
	return -position & (alignment - 1);
}
