export const toBase64 = (bytes: Uint8Array): string => {
    let binary = '';

    for (let i = 0; i < bytes.length; i++) {
        binary += String.fromCharCode(bytes[i])        
    }

    return btoa(binary);
};

export const fromBase64 = (base64: string): Uint8Array => {
    const binaryString = atob(base64.replace(/-/g, '+').replace(/_/g, '/'));
    const bytes = new Uint8Array(binaryString.length);

    for (let i = 0; i < binaryString.length; i++) {
        bytes[i] = binaryString.charCodeAt(i);        
    }

    return bytes;
};