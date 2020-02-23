<template>
  <SelectControl
    :model="model"
    :values="['none', 'read', 'write']"
    :label="translationPath"
    :description="showDescription ? translationPath + 'Description' : false"
    itemString="core."
    @change="emitChange"
  />
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop, Emit } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import SelectControl from './SelectControl.vue'

@Component({
  components: {
    SelectControl,
  },
})
export default class PermissionControl extends Vue {
  @Prop(Object)
  private readonly model?: { value: string }

  @Prop(Array)
  private readonly values: string[] = []

  @Prop(String)
  private readonly translationPath?: string

  @Prop(Boolean)
  private readonly showDescription?: boolean

  @Emit('change')
  private emitChange() {
    return this.model?.value
  }
}
</script>
