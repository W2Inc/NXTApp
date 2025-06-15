export interface FileNode {
  name: string;
  type: 'file' | 'directory';
  extension?: string;
  content?: string;
  path: string;
  children?: FileNode[];
}
