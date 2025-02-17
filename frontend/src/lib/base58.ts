/**
 * Base58 implementation for TypeScript
 * Created: 2025-02-17
 * Author: W2Wizard
 */

export class Base58 {
	// Base58 alphabet (Bitcoin)
	private static readonly ALPHABET =
		"123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
	private static readonly BASE = Base58.ALPHABET.length;
	private static readonly LEADER = Base58.ALPHABET.charAt(0);

	// Pre-compute lookup table
	private static readonly INDEXES = new Uint8Array(128);

	static {
		// Initialize all to -1
		Base58.INDEXES.fill(-1);

		// Only compute indexes for chars in the alphabet
		for (let i = 0; i < Base58.ALPHABET.length; i++) {
			Base58.INDEXES[Base58.ALPHABET.charCodeAt(i)] = i;
		}
	}

	/**
	 * Encode a Uint8Array to Base58
	 * @param bytes The bytes to encode
	 * @returns The Base58 encoded string
	 */
	public static encode(bytes: Uint8Array): string {
		if (bytes.length === 0) return "";

		// Count leading zeros
		let zeros = 0;
		while (zeros < bytes.length && bytes[zeros] === 0) {
			zeros++;
		}

		// Convert to a big number
		let num = BigInt(0);
		for (let i = zeros; i < bytes.length; i++) {
			num = num * BigInt(256) + BigInt(bytes[i]);
		}

		// Convert to base58 string
		let encoded = "";
		while (num > BigInt(0)) {
			const div = Number(num % BigInt(Base58.BASE));
			encoded = Base58.ALPHABET.charAt(div) + encoded;
			num = num / BigInt(Base58.BASE);
		}

		// Add leading zeros back
		for (let i = 0; i < zeros; i++) {
			encoded = Base58.LEADER + encoded;
		}

		return encoded;
	}

	/**
	 * Decode a Base58 string to a Uint8Array
	 * @param encoded The Base58 encoded string
	 * @returns The decoded bytes
	 * @throws Error if the string contains invalid characters
	 */
	public static decode(encoded: string): Uint8Array {
		if (encoded.length === 0) return new Uint8Array(0);

		// Count leading '1's
		let zeros = 0;
		while (zeros < encoded.length && encoded[zeros] === Base58.LEADER) {
			zeros++;
		}

		// Convert from Base58 string to number
		let num = BigInt(0);
		for (let i = zeros; i < encoded.length; i++) {
			const charCode = encoded.charCodeAt(i);
			const index = charCode < 128 ? Base58.INDEXES[charCode] : -1;

			if (index === -1) {
				throw new Error(`Invalid Base58 character '${encoded[i]}' at position ${i}`);
			}

			num = num * BigInt(Base58.BASE) + BigInt(index);
		}

		// Convert to byte array
		const bytes: number[] = [];
		while (num > BigInt(0)) {
			bytes.unshift(Number(num & BigInt(255)));
			num = num >> BigInt(8);
		}

		// Add leading zeros back
		for (let i = 0; i < zeros; i++) {
			bytes.unshift(0);
		}

		return new Uint8Array(bytes);
	}

	/**
	 * Encode a UUID to Base58
	 * @param uuid The UUID string to encode
	 * @returns The Base58 encoded string
	 */
	public static encodeUUID(uuid: string): string {
		// Remove hyphens and convert to bytes
		const hex = uuid.replace(/-/g, "");
		const bytes = new Uint8Array(16);

		for (let i = 0; i < 16; i++) {
			bytes[i] = parseInt(hex.slice(i * 2, i * 2 + 2), 16);
		}

		return Base58.encode(bytes);
	}

	/**
	 * Decode a Base58 string back to a UUID
	 * @param encoded The Base58 encoded string
	 * @returns The UUID string
	 */
	public static decodeToUUID(encoded: string): string {
		const bytes = Base58.decode(encoded);
		if (bytes.length !== 16) {
			throw new Error("Invalid Base58 UUID encoding");
		}

		// Convert bytes back to UUID string format
		const hex = Array.from(bytes)
			.map((b) => b.toString(16).padStart(2, "0"))
			.join("");

		return [
			hex.slice(0, 8),
			hex.slice(8, 12),
			hex.slice(12, 16),
			hex.slice(16, 20),
			hex.slice(20, 32),
		].join("-");
	}
}
