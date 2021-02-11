<template>
    <div class="card" v-if="node != null">
        <div
            :class="{
                'card-header': true,
                added: diffStatus === 'added',
                deleted: diffStatus === 'deleted',
                modified: diffStatus === 'modified'
            }"
            id="headingOne"
            >
            <h2 class="mb-0">
                <button
                    class="btn dropdown-toggle btn-block text-left"
                    type="button"
                    v-on:click="showBody = !showBody"
                    >
                    {{ node[0] }}
                </button>
            </h2>
        </div>
        <div
            v-if="node[1] && showBody"
            v-show="showBody"
            >
            <div class="card-body" v-if="typeof node[1] == 'string'">
                <component v-if="scalarComponent" :is="scalarComponent" :value="node[1]"></component>
                <span v-else>{{ node[1] }}</span>
            </div>
            <div class="card-body" v-else-if="typeof node[1].entries == 'function'">
                <Tree
                    :nodes="node[1].entries()"
                    :nodeComponent="nodeComponent"
                    :scalarComponent="scalarComponent"
                    ></Tree>
            </div>
            <div class="card-body leaf" v-else>
                <table class="table table-striped">
                    <thead>
                        <th>Field</th>
                        <th>Value</th>
                    </thead>
                    <tbody>
                        <template v-for="(value, key) in node[1]">
                            <tr v-bind:key="key + 'tr'" v-if="showTableRow(key, value)">
                                <td>{{ key }}</td>
                                <td v-if="scalarComponent">
                                    <component :is="scalarComponent" :value="value"></component>
                                </td>
                                <td v-else>{{ value }}</td>
                            </tr>
                        </template>
                    </tbody>
                </table>

                <div class="values" v-if="showValues">
                    <Tree
                        :nodes="values"
                        :nodeComponent="nodeComponent"
                        :scalarComponent="scalarComponent"
                        ></Tree>
                </div>

                <div class="properties" v-if="hasProperties">
                    <Tree
                        :nodes="getProperties()"
                        :nodeComponent="nodeComponent"
                        :scalarComponent="scalarComponent"
                        ></Tree>
                </div>
            </div>
        </div>
    </div>
</template>

<script src="@/components/Tree/TreeNode/TreeNode.js"></script>

<style lang="sass" scoped src="@/components/Tree/TreeNode/TreeNode.sass"></style>
