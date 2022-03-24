export interface KidActivity {
    id: number;
    name: string;
    description: string;
    picture: string;
    videoClip: string;
}

export interface KidActivityCreateEdit {
    id: number;
    name: string;
    description: string;
    picture: File;
    videoClip: string;
}

