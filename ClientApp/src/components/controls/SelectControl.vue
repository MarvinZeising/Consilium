<template>
  <v-flex xs12 sm6 md4 class="pa-2">
    <span v-if="description" v-t="description" />
    <v-select
      v-model="model.value"
      :items="getValues"
      item-value="value"
      :label="$tc(label, 2)"
      hide-details
      filled
      required
      @change="emitChange"
    >
      <template v-slot:selection="{ item }">
        <span>{{ $t(itemString + item.value) }}</span>
      </template>
      <template v-slot:item="{ item }">
        <span>{{ $t(itemString + item.value) }}</span>
      </template>
    </v-select>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop, Emit } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'

@Component
export default class PermissionControl extends Vue {
  @Prop(Object)
  private readonly model?: { value: string }

  @Prop(Array)
  private readonly values?: string[]

  @Prop(String)
  private readonly label?: string

  @Prop(String)
  private readonly description?: string

  @Prop(String)
  private readonly itemString?: string

  private get getValues() {
    return this.values?.map((value) => {
      return { value }
    })
  }

  @Emit('change')
  private emitChange() {
    return this.model?.value
  }
}
</script>
