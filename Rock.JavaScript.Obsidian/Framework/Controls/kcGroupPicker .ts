import { defineCustomElement } from "@Obsidian/Utility/component";
import { BaseAsyncPicker } from "@Obsidian/Controls/baseAsyncPicker";
import { kcGroupPicker } from "../../Framework/Controls/kcGroupPicker";
import { ref } from "vue";

export const kcGroupPicker = defineCustomElement({
    name: "kc-group-picker",

    props: {
        modelValue: {
            type: String,
            default: "",
        },
        includeInactive: {
            type: Boolean,
            default: false,
        },
    },

    setup(props, { emit }) {
        const internalValue = ref(props.modelValue);

        const getItems = async () => {
            const url = `/api/v2/controls/groups?includeInactive=${props.includeInactive}`;
            const response = await fetch(url);
            const groups = await response.json();

            return groups.map((g: any) => ({
                text: g.name,
                value: g.guid,
            }));
        };

        const updateValue = (newValue: string) => {
            internalValue.value = newValue;
            emit("update:modelValue", newValue);
        };

        return () => (
            <BaseAsyncPicker
                label="Select a Group"
                v-model={internalValue.value}
                getItems={getItems}
                onUpdate:modelValue={updateValue}
            />
        );
    },
});
